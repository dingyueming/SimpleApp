﻿using Simple.IDomain;
using Simple.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Simple.Infrastructure.InfrastructureModel.ZTree;
using AutoMapper;
using Simple.ExEntity.Map;
using Simple.Infrastructure.InfrastructureModel.VueTreeSelect;
using Simple.Infrastructure.Tools;
using Simple.Entity;
using System;
using Simple.Infrastructure.QueryModels;

namespace Simple.Domain
{
    /// <summary>
    /// 地图显示domain
    /// </summary>
    public class MapShowDomainService : IMapShowDomainService
    {
        private readonly IXfSyxxRepository xfSyxxRepository;
        private readonly IXfKeyUnitRepository xfKeyUnitRepository;
        private readonly ISjtlAttendancePositionRepository sjtlAttendancePositionRepository;
        private readonly ISjgx110AlarmRepository sjgx110AlarmRepository;
        private readonly INewtrackRepository newtrackRepository;
        private readonly IViewAllTargetRepository viewAllTargetRepository;
        private readonly ILastLocatedRepository lastLocatedRepository;
        private readonly ICarRepository carRepository;
        private readonly IPersonRepository personRepository;
        private readonly IUnitRepository unitRepository;
        private readonly IMapper mapper;
        public MapShowDomainService(IXfKeyUnitRepository xfKeyUnitRepository, IXfSyxxRepository xfSyxxRepository,
            ISjtlAttendancePositionRepository sjtlAttendancePositionRepository, ISjgx110AlarmRepository sjgx110AlarmRepository,
            INewtrackRepository newtrackRepository, IViewAllTargetRepository viewAllTargetRepository, ILastLocatedRepository lastLocatedRepository,
            IPersonRepository personRepository, ICarRepository carRepository, IUnitRepository unitRepository, IMapper mapper)
        {
            this.xfSyxxRepository = xfSyxxRepository;
            this.xfKeyUnitRepository = xfKeyUnitRepository;
            this.sjtlAttendancePositionRepository = sjtlAttendancePositionRepository;
            this.sjgx110AlarmRepository = sjgx110AlarmRepository;
            this.newtrackRepository = newtrackRepository;
            this.viewAllTargetRepository = viewAllTargetRepository;
            this.lastLocatedRepository = lastLocatedRepository;
            this.personRepository = personRepository;
            this.carRepository = carRepository;
            this.unitRepository = unitRepository;
            this.mapper = mapper;
        }

        public async Task<TreeNode[]> GetDeviceTreeByUser(int userId)
        {
            //获取所有单位
            var allUnits = await unitRepository.GetAllAsync();
            allUnits = allUnits.OrderBy(x => x.UNITNAME).ToList();
            //获取当前用户所拥有的设备
            var devices = await carRepository.GetCarEntitiesByUser(userId);
            //组织tree
            var listNode = new List<TreeNode>();
            foreach (var unit in allUnits)
            {
                var treeNode = new TreeNode()
                {
                    name = unit.UNITNAME,
                    id = $"unit-{ unit.UNITID }",
                    pId = $"unit-{unit.PID}",
                    isParent = true,
                    iconSkin = "pIcon01"
                };
                var flag = allUnits.Any(x => x.PID == unit.UNITID) || devices.Any(o => o.UNITID == unit.UNITID);
                if (!flag)
                {
                    treeNode.isParent = false;
                    treeNode.iconSkin = "icon10";
                }
                listNode.Add(treeNode);
            }
            foreach (var device in devices)
            {
                var treeNode = new TreeNode()
                {
                    name = device.CARNO,
                    pId = $"unit-{device.UNITID}",
                    id = $"car-{ device.CARID }",
                    iconSkin = "gray_car"
                };
                listNode.Add(treeNode);
            }
            foreach (var item in listNode)
            {
                //循环单位,给单位加上下属设备的数量
                if (!item.id.Contains("car"))
                {
                    item.name += $"({ RecursiveTreeNode(item, listNode)}/0)";
                }
            }
            return listNode.Where(x => x.name.Contains("(0/0)") == false).ToArray();
        }

        private int RecursiveTreeNode(TreeNode treeNode, List<TreeNode> treeNodes)
        {
            //获取当前节点下的设备数量
            var deviceCount = treeNodes.Where(x => x.pId == treeNode.id).Where(x => x.id.Contains("car")).ToList().Count;
            //获取当前节点下的单位集合
            var childUnit = treeNodes.Where(x => x.pId == treeNode.id).Where(x => x.id.Contains("unit")).ToList();
            //当前节点下的单位数量
            var unitCount = childUnit.Count;
            //子节点用后的设备数据量
            var childUnitsDeviceCount = 0;
            //大于0说明有子单位
            if (unitCount > 0)
            {
                //递归
                foreach (var item in childUnit)
                {
                    childUnitsDeviceCount += RecursiveTreeNode(item, treeNodes);
                }
            }
            return deviceCount + childUnitsDeviceCount;
        }

        public async Task<VueTreeSelectModel[]> GetVueDeviceTreeByUser(int userId)
        {
            var allNodes = await GetDeviceTreeByUser(userId);
            //过滤掉部门名称后边的总数和在线数
            foreach (var item in allNodes)
            {
                if (item.id.Contains("unit-"))
                {
                    item.name = item.name.Remove(item.name.IndexOf("("), item.name.Length - item.name.IndexOf("("));
                }
            }
            var node = allNodes.FirstOrDefault(x => x.pId == "unit-0");
            var arrTreeSelectModel = TreeHelper.GetTreeSelectModels(allNodes.ToList(), node);
            return arrTreeSelectModel.ToArray();
        }

        public async Task<List<PersonExEntity>> GetPersonEntitiesByUser(int userId)
        {
            var persons = await personRepository.GetPersonEntitiesByUser(userId);
            var personExEntities = mapper.Map<List<PersonExEntity>>(persons);
            return personExEntities;
        }

        public async Task<List<CarExEntity>> GetCarEntitiesByUser(int userId)
        {
            var cars = await carRepository.GetCarEntitiesByUser(userId);
            var carExEntiies = mapper.Map<List<CarExEntity>>(cars);
            return carExEntiies;
        }

        public async Task<List<LastLocatedExEntity>> GetLastLocatedByUser(int userId)
        {
            var lastLocateds = await lastLocatedRepository.GetLastLocatedEntityByUser(userId);
            var exEntities = mapper.Map<List<LastLocatedExEntity>>(lastLocateds);
            return exEntities;
        }

        public async Task<List<ViewAllTargetExEntity>> GetAllDeviceByUser(int userId)
        {
            var viewAllTargetEntities = await viewAllTargetRepository.GetDevicesByUser(userId);
            var exEntities = mapper.Map<List<ViewAllTargetExEntity>>(viewAllTargetEntities);
            return exEntities;
        }

        public async Task<List<NewTrackExEntity>> GetNewTrackList(dynamic qm)
        {
            var entities = await newtrackRepository.GetNewtracksByDeviceId(qm);
            var exEntities = mapper.Map<List<NewTrackExEntity>>(entities);
            foreach (NewTrackExEntity item in exEntities)
            {
                CarStatus carStatus = new CarStatus();
                carStatus.RefreshStatus(item.STATUS, 0);
                item.StatusShow = carStatus.ToString();
            }
            return exEntities;
        }

        public async Task<List<Sjgx110AlarmExEntity>> GetSjgx110AlarmExEntities(Sjgx110AlarmQm qm)
        {
            var entities = await sjgx110AlarmRepository.GetAlarmEntities(qm.DateTimes[0], qm.DateTimes[1], qm.Points.Count == 4 ? qm.Points[0] : null, qm.Points.Count == 4 ? qm.Points[2] : null);
            return mapper.Map<List<Sjgx110AlarmExEntity>>(entities);
        }

        public async Task<List<SjtlAttendancePositionExEntity>> GetSjtlAttenPosExEnties(SjtlAttPosQm qm)
        {
            var entities = await sjtlAttendancePositionRepository.GetEntities(qm.DateTimes[0], qm.DateTimes[1], qm.Points.Count == 4 ? qm.Points[0] : null, qm.Points.Count == 4 ? qm.Points[2] : null, qm.NameOrPoliceCode);
            return mapper.Map<List<SjtlAttendancePositionExEntity>>(entities);
        }

        public async Task<List<XfKeyUnitExEntity>> GetXfKeyUnitExEntities()
        {
            var entities = await xfKeyUnitRepository.GetAllAsync();
            return mapper.Map<List<XfKeyUnitExEntity>>(entities);
        }

        public async Task<List<XfSyxxExEntity>> GetXfSyxxExEntities()
        {
            var entities = await xfSyxxRepository.GetAllAsync();
            return mapper.Map<List<XfSyxxExEntity>>(entities);
        }
    }
}
