using AutoMapper;
using Simple.Entity;
using Simple.ExEntity.DM;
using Simple.IDomain;
using Simple.Infrastructure.InfrastructureModel.Element;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Infrastructure.InfrastructureModel.VueTreeSelect;
using Simple.Infrastructure.InfrastructureModel.ZTree;
using Simple.Infrastructure.Tools;
using Simple.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Domain
{
    public class DmDomainService : IDmDomainService
    {
        #region 构造函数
        private readonly IAuthLimitRepository authLimitRepository;
        private readonly IPersonRepository personRepository;
        private readonly ICarRepository carRepository;
        private readonly IUnitRepository unitRepository;
        private readonly IViewAllTargetRepository viewAllTargetRepository;
        private readonly IMapper mapper;
        public DmDomainService(IAuthLimitRepository authLimitRepository, IViewAllTargetRepository viewAllTargetRepository,
            IPersonRepository personRepository, ICarRepository carRepository, IUnitRepository unitRepository, IMapper mapper)
        {
            this.authLimitRepository = authLimitRepository;
            this.viewAllTargetRepository = viewAllTargetRepository;
            this.personRepository = personRepository;
            this.carRepository = carRepository;
            this.unitRepository = unitRepository;
            this.mapper = mapper;
        }
        #endregion

        #region 单位管理
        public async Task<bool> AddUnit(UnitExEntity exEntity)
        {
            var entity = mapper.Map<UnitEntity>(exEntity);
            return await unitRepository.InsertAsync(entity);
        }

        public async Task<bool> DeleteUnit(List<UnitExEntity> exEntities)
        {
            var entities = mapper.Map<List<UnitEntity>>(exEntities);
            return await unitRepository.DeleteAsync(entities);
        }

        public async Task<bool> UpdateUnit(UnitExEntity exEntity)
        {
            var entity = mapper.Map<UnitEntity>(exEntity);
            return await unitRepository.UpdateAsync(entity);
        }

        public async Task<Pagination<UnitExEntity>> GetUnitPage(Pagination<UnitExEntity> param)
        {
            var pagination = await unitRepository.GetPage(param.PageSize, param.PageIndex, param.Where, param.OrderBy);
            return mapper.Map<Pagination<UnitExEntity>>(pagination);
        }
        public async Task<VueTreeSelectModel[]> GetUnitTree()
        {
            var allUnits = await unitRepository.GetAllAsync();
            var treeSelectModels = GetUnitTreeSelectModels(allUnits.ToList(), null);
            return treeSelectModels.ToArray();
        }
        public async Task<ElementTreeModel[]> GetUnitAndDeviceTree()
        {
            var allUnits = await unitRepository.GetAllAsync();
            var allDevices = await carRepository.GetAllAsync();
            var elementModels = GetUnitTreeModels(allUnits.ToList(), null, allDevices.ToList());
            return elementModels.ToArray();
        }
        #endregion

        #region 车辆管理

        public async Task<bool> AddCar(CarExEntity exEntity)
        {
            var entity = mapper.Map<CarEntity>(exEntity);
            //数据校验
            var valdataEntity = await carRepository.GetCarEntityForValdata(entity);
            if (valdataEntity != null)
            {
                if (valdataEntity.SIM == entity.SIM)
                {
                    throw new Exception("SIM卡号重复");
                }
                if (valdataEntity.LICENSE == entity.LICENSE)
                {
                    throw new Exception("车牌号重复");
                }
                if (valdataEntity.MAC == entity.MAC)
                {
                    throw new Exception("识别码重复");
                }
            }
            return await carRepository.InsertAsync(entity);
        }

        public async Task<bool> DeleteCar(List<CarExEntity> exEntities)
        {
            var entities = mapper.Map<List<CarEntity>>(exEntities);
            return await carRepository.DeleteAsync(entities);
        }

        public async Task<bool> UpdateCar(CarExEntity exEntity)
        {
            var entity = mapper.Map<CarEntity>(exEntity);
            //数据校验
            var valdataEntity = await carRepository.GetCarEntityForValdata(entity);
            if (valdataEntity != null && valdataEntity.CARID != exEntity.CARID)
            {
                if (valdataEntity.SIM == entity.SIM)
                {
                    throw new Exception("SIM卡号重复");
                }
                if (valdataEntity.LICENSE == entity.LICENSE)
                {
                    throw new Exception("车牌号重复");
                }
                if (valdataEntity.MAC == entity.MAC)
                {
                    throw new Exception("识别码重复");
                }
            }
            return await carRepository.UpdateAsync(entity);
        }

        public async Task<Pagination<CarExEntity>> GetCarPage(Pagination<CarExEntity> param)
        {
            var pagination = await carRepository.GetPage(param.PageSize, param.PageIndex, param.Where, param.OrderBy);
            return mapper.Map<Pagination<CarExEntity>>(pagination);
        }

        #endregion

        #region 人员对讲机管理

        public async Task<bool> AddPerson(PersonExEntity exEntity)
        {
            var entity = mapper.Map<PersonEntity>(exEntity);
            //数据校验
            var valdataEntity = await personRepository.GetPersonEntityForValdata(entity);
            if (valdataEntity != null)
            {
                if (valdataEntity.POLICE_CODE == entity.POLICE_CODE && exEntity.POLICE_CODE.Trim() != null)
                {
                    throw new Exception("警号重复");
                }
                if (valdataEntity.TERMINAL_CODE == entity.TERMINAL_CODE)
                {
                    throw new Exception("设备号重复");
                }
            }
            return await personRepository.InsertAsync(entity);
        }

        public async Task<bool> DeletePerson(List<PersonExEntity> exEntities)
        {
            var entities = mapper.Map<List<PersonEntity>>(exEntities);
            return await personRepository.DeleteAsync(entities);
        }

        public async Task<bool> UpdatePerson(PersonExEntity exEntity)
        {
            var entity = mapper.Map<PersonEntity>(exEntity);
            //数据校验
            var valdataEntity = await personRepository.GetPersonEntityForValdata(entity);
            if (valdataEntity != null)
            {
                if (valdataEntity.POLICE_CODE == entity.POLICE_CODE && exEntity.POLICE_CODE.Trim() != null)
                {
                    throw new Exception("警号重复");
                }
                if (valdataEntity.TERMINAL_CODE == entity.TERMINAL_CODE)
                {
                    throw new Exception("设备号重复");
                }
            }
            return await personRepository.UpdateAsync(entity);
        }

        public async Task<Pagination<PersonExEntity>> GetPersonPage(Pagination<PersonExEntity> param)
        {
            var pagination = await personRepository.GetPage(param.PageSize, param.PageIndex, param.Where, param.OrderBy);
            return mapper.Map<Pagination<PersonExEntity>>(pagination);
        }

        #endregion

        #region 设备分配

        public async Task<string[]> GetDeviceIdsByUser(int userId)
        {
            var list = new List<string>();
            var devices = await carRepository.GetCarEntitiesByUser(userId);
            var deviceIds = devices.Select(x => x.CARID).ToList();
            devices.ForEach((x) =>
            {
                list.Add("device" + x.CARID);
            });
            return list.ToArray();
        }

        public async Task<bool> UpdateAuthLimits(List<ElementTreeModel> nodes, int userId)
        {
            var entities = new List<AuthLimitsEntity>();
            foreach (var item in nodes)
            {
                if (item.id.Contains("device"))
                {
                    entities.Add(new AuthLimitsEntity()
                    {
                        UserId = userId,
                        IsSendad = 1,
                        CarId = int.Parse(item.id.Replace("device", ""))
                    });
                }
            }
            return await authLimitRepository.UpdateAuthLimits(entities, userId);
        }

        #endregion

        #region 递归单位tree

        public static List<VueTreeSelectModel> GetUnitTreeSelectModels(List<UnitEntity> allNodes, UnitEntity node)
        {
            var list = new List<VueTreeSelectModel>();
            if (node == null)
            {
                var unit = allNodes.FirstOrDefault(x => x.PID == -1);
                var nodeChildren = GetUnitTreeSelectModels(allNodes, unit);
                var firstNode = new VueTreeSelectModel() { id = unit.UNITID.ToString(), label = unit.UNITNAME, Tag = unit.ORG_CODE, children = nodeChildren?.ToArray() };
                list.Add(firstNode);
            }
            else
            {
                var nodes = allNodes.Where(x => x.PID == node.UNITID).ToList();
                if (nodes.Count > 0)
                {
                    nodes.ForEach((x) =>
                    {
                        var nodeChildren = GetUnitTreeSelectModels(allNodes, x);
                        list.Add(new VueTreeSelectModel() { id = x.UNITID.ToString(), label = x.UNITNAME, Tag = x.ORG_CODE, children = nodeChildren?.ToArray() });
                    });
                }
                else
                {
                    return null;
                }
            }

            return list;
        }
        public static List<ElementTreeModel> GetUnitTreeModels(List<UnitEntity> allNodes, UnitEntity node, List<CarEntity> allDevice)
        {
            var list = new List<ElementTreeModel>();
            if (node == null)
            {
                var unit = allNodes.FirstOrDefault(x => x.PID == -1);
                var nodeChildren = GetUnitTreeModels(allNodes, unit, allDevice);
                if (nodeChildren != null && nodeChildren.Count > 0)
                {
                    var firstNode = new ElementTreeModel() { id = "unit" + unit.UNITID.ToString(), label = unit.UNITNAME, children = nodeChildren?.ToArray() };
                    list.Add(firstNode);
                }
            }
            else
            {
                var nodes = allNodes.Where(x => x.PID == node.UNITID).ToList();
                var devices = allDevice.Where(x => x.UNITID == node.UNITID).ToList();
                if (nodes.Count > 0 || devices.Count > 0)
                {
                    nodes.ForEach((x) =>
                    {
                        var nodeChildren = GetUnitTreeModels(allNodes, x, allDevice);
                        if (nodeChildren != null && nodeChildren.Count > 0)
                        {
                            list.Add(new ElementTreeModel() { id = "unit" + x.UNITID.ToString(), label = x.UNITNAME, children = nodeChildren?.ToArray() });
                        }
                    });
                    devices.ForEach((x) =>
                    {
                        list.Add(new ElementTreeModel() { id = "device" + x.CARID.ToString(), label = x.LICENSE, children = null });
                    });
                }
                else
                {
                    return null;
                }
            }

            return list;
        }

        #endregion
    }
}
