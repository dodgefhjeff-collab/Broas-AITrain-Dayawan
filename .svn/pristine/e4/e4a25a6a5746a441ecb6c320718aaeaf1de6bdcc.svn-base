using System;
using System.Threading.Tasks;
using SqlSugar;
using WaterStation.Client.Model.Commom;
using WaterStation.Client.Model.LayoutDto;
using System.Collections.Generic;
using WaterStation.Client.Model.RunLogDto;
using WaterStation.Client.Model.UserDto;
using System.Linq;
using NPOI.SS.Formula.Functions;
using NLog;
namespace WaterStation.Client.Controller
{
    public class DataAPI
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public static string dbConnectSting = @"DataSource= "+ AppDomain.CurrentDomain.BaseDirectory+ "BroasAITrain.db";
        private static Snowflake snowflake = new();
        private static volatile bool _measureDataIndexesEnsured;
        private static readonly object MeasureDataIndexLock = new();
        private static volatile bool _qcRecordSchemaEnsured;
        private static readonly object QCRecordSchemaLock = new();

        private static SqlSugarClient ClientConct()
        {
            var client = new SqlSugarClient(new ConnectionConfig
            {
                DbType = DbType.Sqlite,
                ConnectionString = dbConnectSting,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
            EnsureMeasureDataIndexes(client);
            EnsureQCRecordSchema(client);
            return client;
        }

        /// <summary>
        /// 确保质控记录表及菜单项存在（幂等，仅首次执行）
        /// </summary>
        private static void EnsureQCRecordSchema(SqlSugarClient cc)
        {
            if (_qcRecordSchemaEnsured)
                return;

            lock (QCRecordSchemaLock)
            {
                if (_qcRecordSchemaEnsured)
                    return;

                try
                {
                    cc.CodeFirst.InitTables<QCRecord>();
                    EnsureQCRecordMenu(cc);
                    _qcRecordSchemaEnsured = true;
                    _logger.Info("QCRecord 表及质控记录菜单已创建或已存在");
                }
                catch (Exception ex)
                {
                    _logger.Warn(ex, "创建 QCRecord 表或质控记录菜单失败");
                }
            }
        }

        /// <summary>
        /// 幂等注册「质控记录」菜单及角色权限
        /// </summary>
        private static void EnsureQCRecordMenu(SqlSugarClient cc)
        {
            const string menuName = "质控记录";
            const string menuUrl = "LayoutManageMoudle.QCData";
            const string legacyUrl = "BatteryManageMoudle.QualityControl";

            var matchedFunctions = cc.Queryable<FunctionInfo>()
                .Where(f => f.FullName == menuName || f.URL == menuUrl || f.URL == legacyUrl || f.Code == "QCData")
                .ToList();

            long functionId;
            if (matchedFunctions.Count > 0)
            {
                functionId = matchedFunctions.Min(f => f.ID);
                var duplicateIds = matchedFunctions.Where(f => f.ID != functionId).Select(f => f.ID).ToList();

                cc.Updateable<FunctionInfo>()
                    .SetColumns(it => new FunctionInfo { URL = menuUrl, FullName = menuName, Code = "QCData" })
                    .Where(it => it.ID == functionId)
                    .ExecuteCommand();

                if (duplicateIds.Count > 0)
                {
                    foreach (var dupId in duplicateIds)
                    {
                        var dupBindings = cc.Queryable<RoleFunctionInfo>().Where(r => r.FunctionID == dupId).ToList();
                        foreach (var binding in dupBindings)
                        {
                            var hasCanonical = cc.Queryable<RoleFunctionInfo>()
                                .Any(r => r.RoleID == binding.RoleID && r.FunctionID == functionId);
                            if (hasCanonical)
                                cc.Deleteable<RoleFunctionInfo>().Where(r => r.ID == binding.ID).ExecuteCommand();
                            else
                                cc.Updateable<RoleFunctionInfo>()
                                    .SetColumns(it => new RoleFunctionInfo { FunctionID = functionId })
                                    .Where(it => it.ID == binding.ID)
                                    .ExecuteCommand();
                        }
                        cc.Deleteable<FunctionInfo>().Where(f => f.ID == dupId).ExecuteCommand();
                    }
                    _logger.Info("已合并删除重复质控记录菜单 {Count} 条", duplicateIds.Count);
                }
            }
            else
            {
                functionId = snowflake.GetId();
                cc.Insertable(new FunctionInfo
                {
                    ID = functionId,
                    Code = "QCData",
                    FullName = menuName,
                    URL = menuUrl,
                    Description = "质控记录查询",
                    Operation = 0,
                    Symbol = 61573
                }).ExecuteCommand();
            }

            var parent = cc.Queryable<FunctionInfo>().Where(f => f.FullName == "后台管理").ToList().FirstOrDefault();
            long parentRoleFunctionId = 0;
            int treeIndex = 99;
            if (parent != null)
            {
                var parentRoleFunc = cc.Queryable<RoleFunctionInfo>()
                    .Where(r => r.FunctionID == parent.ID && r.ParentID == 0)
                    .ToList()
                    .FirstOrDefault();
                if (parentRoleFunc != null)
                {
                    parentRoleFunctionId = parentRoleFunc.ID;
                    var childFuncs = cc.Queryable<RoleFunctionInfo>()
                        .Where(r => r.ParentID == parentRoleFunctionId)
                        .ToList();
                    treeIndex = childFuncs.Count > 0 ? childFuncs.Max(r => r.TreeIndex) + 1 : 1;
                }
            }

            var roles = cc.Queryable<RoleInfo>().ToList();
            foreach (var role in roles)
            {
                var roleFunc = cc.Queryable<RoleFunctionInfo>()
                    .Where(r => r.RoleID == role.ID && r.FunctionID == functionId)
                    .ToList()
                    .FirstOrDefault();
                if (roleFunc != null)
                {
                    if (parentRoleFunctionId > 0 && roleFunc.ParentID != parentRoleFunctionId)
                    {
                        cc.Updateable<RoleFunctionInfo>()
                            .SetColumns(it => new RoleFunctionInfo { ParentID = parentRoleFunctionId, TreeIndex = treeIndex })
                            .Where(it => it.ID == roleFunc.ID)
                            .ExecuteCommand();
                    }
                    continue;
                }

                var hasQcMenuForRole = cc.Queryable<RoleFunctionInfo, FunctionInfo>((r, f) => new JoinQueryInfos(JoinType.Inner, r.FunctionID == f.ID))
                    .Where((r, f) => r.RoleID == role.ID
                        && (f.FullName == menuName || f.URL == menuUrl || f.Code == "QCData"))
                    .Any();
                if (hasQcMenuForRole)
                    continue;

                cc.Insertable(new RoleFunctionInfo
                {
                    ID = snowflake.GetId(),
                    RoleID = role.ID,
                    FunctionID = functionId,
                    ParentID = parentRoleFunctionId,
                    TreeIndex = treeIndex,
                    IsPopUpWindow = false,
                    Operation = 0
                }).ExecuteCommand();
            }
        }

        /// <summary>
        /// 为历史数据查询创建复合索引（幂等，仅首次执行）
        /// </summary>
        private static void EnsureMeasureDataIndexes(SqlSugarClient cc)
        {
            if (_measureDataIndexesEnsured)
                return;

            lock (MeasureDataIndexLock)
            {
                if (_measureDataIndexesEnsured)
                    return;

                try
                {
                    cc.Ado.ExecuteCommand(
                        "CREATE INDEX IF NOT EXISTS idx_MeasureData_DataType_CollectTime ON MeasureData(DataType, CollectTime DESC)");
                    cc.Ado.ExecuteCommand(
                        "CREATE INDEX IF NOT EXISTS idx_MeasureData_DataType_Remark_CollectTime ON MeasureData(DataType, Remark, CollectTime DESC)");
                    cc.Ado.ExecuteCommand("ANALYZE MeasureData");
                    _measureDataIndexesEnsured = true;
                    _logger.Info("MeasureData 查询索引已创建或已存在");
                }
                catch (Exception ex)
                {
                    _logger.Warn(ex, "创建 MeasureData 查询索引失败，历史数据查询将回退为全表扫描");
                }
            }
        }

        /// <summary>
        ///系统参数数据查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public async static Task< HeaderResult<List<SystemParaSetInfoDto>>> GetSystemParaAsync(QuerySystemParaSetInfoDto where)
        {
            var cc = ClientConct();
            var exp = Expressionable.Create<SystemParaSetInfo>();
            exp.AndIF(!string.IsNullOrEmpty(where.EquipmentName), a => a.EquipmentName.Contains(where.EquipmentName));
            var result = await cc.Queryable<SystemParaSetInfo>()
                            .Where(exp.ToExpression())
                            .Select((s) => new SystemParaSetInfoDto { ID = s.ID.SelectAll() })
                            .ToListAsync();
            HeaderResult<List<SystemParaSetInfoDto>> headerResult = new() { IsSucceed = true, Result = result, Total = result.Count };
            return headerResult;
        }
       
        public async static Task<List<SystemParaSetInfo>> GetAllSystemParaAsync()
        {
            var cc = ClientConct();
            var result = await cc.Queryable<SystemParaSetInfo>()
                            .ToListAsync();
            return result;
        }

        public async static Task<bool> AddSystemParaAsync(SystemParaSetInfo sysPara)
        {
            var cc = ClientConct();
            var result = await cc.Insertable(sysPara).ExecuteCommandAsync();
            return result> 0;
        }

        public async static Task<bool> UpdateSystemParaAsync(SystemParaSetInfo sysPara)
        {
             var cc = ClientConct();
            var result = await cc.Updateable(sysPara).ExecuteCommandAsync();
            return result > 0;
        }
       
        public async static Task<bool> DeleteSystemParaAsync(long ID)
        {
             var cc = ClientConct();
            int result = await cc.Deleteable<SystemParaSetInfo>().Where(it => it.ID == ID).ExecuteCommandAsync();
            return result>0;
        }
       
        public async static Task<bool> AddMeasureDataAsync(MeasureData data)
        {
            var cc = ClientConct();
            var result = await cc.Insertable(data).ExecuteCommandAsync();
            return result > 0;
        }

        /// <summary>
        /// 质控记录数据查询（条件+分页）
        /// </summary>
        public async static Task<PageDto<ResultQCRecordDto>> GetAllQCRecordAsync(QueryQCRecordDto where)
        {
            var cc = ClientConct();
            RefAsync<int> totalCount = 0;
            var exp = Expressionable.Create<QCRecord>();
            exp.AndIF(where.SatrtTime != new DateTime(1900, 1, 1), a => a.QCTime >= where.SatrtTime);
            exp.AndIF(where.EndTime != new DateTime(1900, 1, 1), a => a.QCTime <= where.EndTime);
            exp.AndIF(!string.IsNullOrWhiteSpace(where.QCName), a => a.QCName.Contains(where.QCName));
            exp.AndIF(!string.IsNullOrWhiteSpace(where.Remark), a => a.Remark == where.Remark);
            var pageDto = new PageDto<ResultQCRecordDto>();
            var result = await cc.Queryable<QCRecord>()
                .Where(exp.ToExpression())
                .OrderBy(a => a.QCTime, OrderByType.Desc)
                .Select(a => new ResultQCRecordDto { ID = a.ID.SelectAll() })
                .ToPageListAsync(where.PageIndex, where.PageSize, totalCount);
            pageDto.IsSucceed = true;
            pageDto.Result = result;
            pageDto.Total = totalCount;
            return pageDto;
        }

        public static bool AddQCData(QCRecord data)
        {
            var cc = ClientConct();
            var result = cc.Insertable(data).ExecuteCommandAsync();
            return result.Result > 0;
        }
        
        private static readonly DateTime MeasureDataQueryMinTime = new(1900, 1, 1);

        /// <summary>
        /// 构建测量数据查询条件（AI数据 / AI周期数据 / 仪表数据共用）
        /// </summary>
        private static Expressionable<MeasureData> BuildMeasureDataExpression(QueryMeasureDataInfoDto where)
        {
            var exp = Expressionable.Create<MeasureData>();
            exp.AndIF(where.SatrtTime > MeasureDataQueryMinTime, a => a.CollectTime >= where.SatrtTime);
            exp.AndIF(where.EndTime > MeasureDataQueryMinTime, a => a.CollectTime <= where.EndTime);
            exp.AndIF(!string.IsNullOrEmpty(where.DataType), a => a.DataType == where.DataType);
            exp.AndIF(!string.IsNullOrEmpty(where.Remark), a => a.Remark == where.Remark);
            return exp;
        }

        /// <summary>
        /// 分页查询测量数据（AI / 仪表数据统一入口）
        /// </summary>
        public async static Task<PageDto<ResultMeasureDataInfoDto>> GetMeasureDataPageAsync(QueryMeasureDataInfoDto where)
        {
            var cc = ClientConct();
            var pageDto = new PageDto<ResultMeasureDataInfoDto>();
            RefAsync<int> totalCount = 0;
            try
            {
                var result = await cc.Queryable<MeasureData>()
                    .Where(BuildMeasureDataExpression(where).ToExpression())
                    .OrderBy(s => s.CollectTime, OrderByType.Desc)
                    .Select(s => new ResultMeasureDataInfoDto { ID = s.ID.SelectAll() })
                    .ToPageListAsync(where.PageIndex, where.PageSize, totalCount);

                pageDto.IsSucceed = true;
                pageDto.Result = result ?? new List<ResultMeasureDataInfoDto>();
                pageDto.Total = totalCount;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "查询测量数据失败 DataType={DataType}, Remark={Remark}", where.DataType, where.Remark);
                pageDto.IsSucceed = false;
                pageDto.Result = new List<ResultMeasureDataInfoDto>();
                pageDto.Total = 0;
            }

            return pageDto;
        }

        public async static Task<PageDto<ResultMeasureDataInfoDto>> GetDeviceDataAsync(QueryMeasureDataInfoDto where)
            => await GetMeasureDataPageAsync(where);

        public async static Task<PageDto<ResultMeasureDataInfoDto>> GetYBDeviceDataAsync(QueryMeasureDataInfoDto where)
            => await GetMeasureDataPageAsync(where);

        public static List<MeasureData> GetHourData(int Shunxu,string DataType)
        {
            var cc = ClientConct();
            var result = cc.Queryable<MeasureData>().Where(i => i.Remark == "小时"&& i.DataType==DataType).OrderBy(a => a.CollectTime, (OrderByType)Shunxu)
                          .ToList();
            return result;
        }

        /// <summary>
        /// 查询所有日志信息
        /// 条件+分页
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public async static Task<PageDto<RunAllLogInfoDto>> GetAllRunLogAsync(QueryRunAllLogInfoDto where)
        {
             var cc = ClientConct();
            var exp = Expressionable.Create<RunAllLogInfo>();
            exp.AndIF(!string.IsNullOrEmpty(where.LogContent), s => s.LogContent.Contains(where.LogContent));
            exp.AndIF(where.ReceivingSatrtTime != new DateTime(1900, 1, 1), s => s.ReceivingTime >= where.ReceivingSatrtTime);
            exp.AndIF(where.ReceivingEndTime != new DateTime(1900, 1, 1), s => s.ReceivingTime <= where.ReceivingEndTime);
            exp.AndIF(!string.IsNullOrEmpty(where.LogType), s => s.LogType == where.LogType);
            PageDto<RunAllLogInfoDto> pageDto = new PageDto<RunAllLogInfoDto>();
            RefAsync<int> totalCount = 0;
            var result = await cc.Queryable<RunAllLogInfo>()
                  .Where(exp.ToExpression())
                  .OrderBy(s => s.ReceivingTime, OrderByType.Desc)
                  .Select((s) => new RunAllLogInfoDto { ID = s.ID.SelectAll() })
                .ToPageListAsync(where.PageIndex, where.PageSize, totalCount);
            pageDto.IsSucceed = true;
            pageDto.Result = result;
            pageDto.Total = totalCount;
            return pageDto;
        }

        public async static Task<bool> AddRunlogAsync(RunAllLogInfoDto sysPara)
        {
             var cc = ClientConct();
            RunAllLogInfo model = new()
            {
                ID = snowflake.GetId(),
                LogContent = sysPara.LogContent,
                LogType = sysPara.LogType,
                ReceivingTime = sysPara.ReceivingTime,
                Addtime = sysPara.Addtime,
                UpdateTime = sysPara.UpdateTime
            };
            var result = await cc.Insertable(model).ExecuteCommandAsync();
            return result > 0;
        }

        /// <summary>
        /// 根据条件查询用户信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public async static Task<List<UserInfoDto>> GetAllUserInfoAsync(UserInfoDto where)
        {
            var cc = ClientConct();
            var exp = Expressionable.Create<UserInfo, RoleInfo>();
            exp.AndIF(!string.IsNullOrEmpty(where.UserName), (it, i) => it.UserName == where.UserName);
            exp.AndIF(!string.IsNullOrEmpty(where.FullName), (it, i) => it.FullName == where.FullName);
            exp.AndIF(!string.IsNullOrEmpty(where.Department), (it, i) => it.Department == where.Department);
            exp.AndIF(!string.IsNullOrEmpty(where.JobNumber), (it, i) => it.JobNumber == where.JobNumber);
            exp.AndIF(true, (it, i) => it.IsDelete == false);
            var result = await cc.Queryable<UserInfo, RoleInfo>((it, i) => new JoinQueryInfos(JoinType.Left, it.RoleID == i.ID))
                    .Where(exp.ToExpression())
                    .Select((it, i) => new UserInfoDto { ID = it.ID.SelectAll(), RoleFullName = i.FullName })
                   .ToListAsync();
            return result;
        }

        private async static Task<bool> CheckUserNameAsync(string name)
        {
            var cc = ClientConct();
            var result = await cc.Queryable<UserInfo>().Where(it => it.UserName == name).ToListAsync();
            return result.Count > 0;
        }
     
        public async static Task<bool> AddUserAsync(UserInfo user)
        {
            var cc = ClientConct();
            int result = 0;
            if (!await CheckUserNameAsync(user.UserName))
            {
                result = await cc.Insertable(user).ExecuteCommandAsync();
            }
            return result > 0;
        }

        public async static Task<bool> UpdateUserAsync(UserInfo user)
        {
            var cc = ClientConct();
            var result = await cc.Updateable(user).ExecuteCommandAsync();
            return result > 0;
        }
      
        public async static Task<bool> DeleteUserAsync(long ID)
        {
             var cc = ClientConct();
            int result = await cc.Deleteable<UserInfo>().Where(it => it.ID == ID).ExecuteCommandAsync();
            return result > 0;
        }
       
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="userNames"></param>
        /// <returns></returns>
        public async static Task<bool> InitPasswordAsync(List<string> userNames)
        {
            var cc = ClientConct();
            string[] alluserName = userNames.ToArray();
            var result = await cc.Updateable<UserInfo>()
                 .SetColumns(it => it.Password == GenerateMD5.CreateMD5("666666"))
                  .Where(it => alluserName.Contains(it.UserName))
                 .ExecuteCommandAsync();

            return result > 0;
        }
      
        /// <summary>
        /// 根据条件查询角色信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public async static Task<List<RoleInfoDto>> GetAllRoleAsync(RoleInfoDto where)
        {
            var cc = ClientConct();
            var exp = Expressionable.Create<RoleInfo>();
            exp.AndIF(!string.IsNullOrEmpty(where.FullName), (it) => it.FullName == where.FullName);
            exp.AndIF(!string.IsNullOrEmpty(where.Code), (it) => it.Code == where.Code);
            var result = await cc.Queryable<RoleInfo>()
                    .Where(exp.ToExpression())
                    .Select((it) => new RoleInfoDto { ID = it.ID.SelectAll()})
                   .ToListAsync();
            return result;
        }

        public async static Task<List<RoleInfoDto>> GetAllRoleListAsync()
        {
            var cc = ClientConct();
            var exp = Expressionable.Create<RoleInfo>();
            var result = await cc.Queryable<RoleInfo>()
                    .Where(exp.ToExpression())
                    .Select((it) => new RoleInfoDto { ID = it.ID.SelectAll() })
                   .ToListAsync();
            return result;
        }

        public async static Task<RoleInfoDto> GetRoleInfoByIDAsync(long id)
        {
            var cc = ClientConct();
            var exp = Expressionable.Create<RoleInfo>();
            var result = await cc.Queryable<RoleInfo>().Where(exp.ToExpression())
            .Select((s) => new RoleInfoDto { ID = id })
            .FirstAsync();
            return result;
        }

        public async static Task<bool> AddRoleAsync(RoleInfo user)
        {
             var cc = ClientConct();
            var  result = await cc.Insertable(user).ExecuteCommandAsync();
            return result > 0;
        }

        public async static Task<bool> UpdateRoleAsync(RoleInfo user)
        {
             var cc = ClientConct();
            var result = await cc.Updateable(user).ExecuteCommandAsync();
            return result > 0;
        }
       
        public async static Task<bool> DeleteRoleAsync(long ID)
        {
           var cc = ClientConct();
            int result = await cc.Deleteable<RoleInfo>().Where(it => it.ID == ID).ExecuteCommandAsync();
            return result > 0;
        }

        public async static Task<List<RoleFunctionInfoDto>> GetRoleFunctionDataByRoleId(long rid)
        {
            var cc = ClientConct();
            var exp = Expressionable.Create<RoleFunctionInfo, FunctionInfo>();
            exp.AndIF(true, (a, b) => a.RoleID == rid);
            var data = await cc.Queryable<RoleFunctionInfo, FunctionInfo>
                             ((a, b) => new JoinQueryInfos(JoinType.Inner, a.FunctionID == b.ID))
                             .Where(exp.ToExpression())
                             .OrderBy((a, b) => a.TreeIndex)
                             .Select((a, b) => new RoleFunctionInfoDto
                             {
                                 ID = a.ID.SelectAll(),
                                 FunctionName = b.FullName,
                             })
                             .ToListAsync();
            return data;
        }

        public async static Task<bool> AddRoleFunctionInfoAsync(RoleFunctionInfoDto roleFunctiondto)
        {
            var cc = ClientConct();
            RoleFunctionInfo roleFunction = new()
            {
                ID = snowflake.GetId(),
                RoleID = roleFunctiondto.RoleID,
                FunctionID = roleFunctiondto.FunctionID,
                ParentID = roleFunctiondto.ParentID,
                TreeIndex = roleFunctiondto.TreeIndex,
                IsPopUpWindow = roleFunctiondto.IsPopUpWindow,
                Operation = roleFunctiondto.Operation
            };
            var result = await cc.Insertable(roleFunction).ExecuteCommandAsync();
            return result > 0 ;
        }

        public async static Task<bool> UpdateRoleFunctionInfoAsync(RoleFunctionInfo roleFunction)
        {
            var cc = ClientConct();
            bool result = await cc.Updateable<RoleFunctionInfo>(roleFunction).UpdateColumns(it => new { it.FunctionID, it.ParentID, it.TreeIndex })
                         .WhereColumns(it => new { it.ID })
                         .ExecuteCommandAsync() > 0;
            return result;
        }

        public async static Task<bool> DeleteRoleFunctionInfoAsync(List<RoleFunctionInfoDto> roleFunctionInfoDtos)
        {
            var cc = ClientConct();
            //判断选中的父级节点子节点是否全选
            var parentIds = roleFunctionInfoDtos.Where(s => s.ParentID == 0).ToList();
            var childIds = roleFunctionInfoDtos.Where(s => s.ParentID != 0).ToList();
            if (parentIds.Any())
            {
                var childsRoleFunctions = await cc.Queryable<RoleFunctionInfo>().Where(s => parentIds.Select(s => s.ID).Contains(s.ParentID))
                .Select(it => new RoleFunctionInfoDto
                {
                    ID = it.ID.SelectAll()
                }).ToListAsync();

                var extistChildIds = childsRoleFunctions.Where(s => !childIds.Select(f => f.ID).Contains(s.ID)).ToList();
                if (extistChildIds.Any())
                    throw new Exception("父节点删除需勾选全部子节点");
            }
            bool re = false;
            foreach(var item in roleFunctionInfoDtos)
            {
                await cc.Deleteable<RoleFunctionInfo>().Where(s => s.ID == item.ID).ExecuteCommandAsync();
                re = true;
            }
            return re;
        }

        /// <summary>
        /// 根据查询子节点信息
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public async static Task<List<RoleFunctionInfoDto>> GetRoleFunctionByRoleidAsync(long roleID)
        {
           var cc = ClientConct();
            return await Task.Run(() => cc.Queryable<RoleFunctionInfo,FunctionInfo>((a,b)=>new JoinQueryInfos(JoinType.Inner, a.FunctionID == b.ID))
             .Select((a,b)=>new RoleFunctionInfoDto { ID = a.ID.SelectAll(),  RoleID= a.RoleID, FunctionID = a.FunctionID, ParentID = a.ParentID, TreeIndex = a.TreeIndex, IsPopUpWindow=a.IsPopUpWindow,FunctionName=b.FullName})
             .ToTree(it => it.Child, it => it.ParentID, 0)
              .Where(it => it.IsPopUpWindow == false && it.RoleID == roleID)
             .OrderBy(it => it.TreeIndex)
            .ToList());
        }
      
        public async static Task<List<RoleFunctionInfo>> GetAllRoleFunctionAsync()
        {
            var cc = ClientConct();
            try 
            {
                return await cc.Queryable<RoleFunctionInfo>().ToListAsync();
            }
            catch
            {
                return null;
            }
        }

        public async static Task<List<FunctionInfo>> GetAllFunctionAsync()
        {
           var cc = ClientConct();
            return await cc.Queryable<FunctionInfo>().ToListAsync();
        }

        public async static Task<UserInfoDto> LoginAsync(string userName, string password)
        {
            var cc = ClientConct();
            var exp = Expressionable.Create<UserInfo>();
            var result = await cc.Queryable<UserInfo>().Where(exp.ToExpression())
                .Select((s) => new UserInfoDto { ID = s.ID.SelectAll() })
             .FirstAsync(it => it.UserName == userName && it.Password == GenerateMD5.CreateMD5(password));
            return result;
        }

        public async static Task<bool> UpdatePasswordAsync(long id, string newPassword)
        {
             var cc = ClientConct();
            newPassword = GenerateMD5.CreateMD5(newPassword);
            var result = await cc.Updateable<UserInfo>()
                  .SetColumns(it => it.Password == newPassword)
                  .Where(it => it.ID == id)
                  .ExecuteCommandAsync();
            if (result > 0) return true;
            else
                return false;
        }

        //获取补发数据
        public async static Task<List<BufaData>> GetBufaData(DateTime SatrtTime, DateTime EndTime)
        {
             var cc = ClientConct();
            var data = await cc.Queryable<MeasureData>()
                             .Select(a => new BufaData 
                             {
                               CollectTime = a.CollectTime,
                               TNVal = a.TotalNitrogenVal,
                               ADVal = a.AmmoniaADValue,
                               CODVal=  a.TotalCODVal,
                               TPVal=  a.TotalPhosphorusVal,
                               TurbidityVal= a.TurbidityVal,
                             }).OrderBy(it => it.CollectTime, OrderByType.Desc)
                             .Where(it=>it.CollectTime>=SatrtTime&&it.CollectTime<=EndTime)
                             .ToListAsync();
            return data;
        }
    }
}
