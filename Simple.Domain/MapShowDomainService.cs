﻿using Simple.IDomain;
using Simple.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Simple.Infrastructure.InfrastructureModel.ZTree;
using AutoMapper;
using Simple.ExEntity.Map;
using Simple.Entity;
using Simple.Infrastructure.InfrastructureModel.VueTreeSelect;
using Simple.Infrastructure.Tools;

namespace Simple.Domain
{
    /// <summary>
    /// 地图显示domain
    /// </summary>
    public class MapShowDomainService : IMapShowDomainService
    {
        private readonly INewtrackRepository newtrackRepository;
        private readonly IViewAllTargetRepository viewAllTargetRepository;
        private readonly ILastLocatedRepository lastLocatedRepository;
        private readonly ICarRepository carRepository;
        private readonly IPersonRepository personRepository;
        private readonly IUnitRepository unitRepository;
        private readonly IMapper mapper;
        public MapShowDomainService(INewtrackRepository newtrackRepository, IViewAllTargetRepository viewAllTargetRepository, ILastLocatedRepository lastLocatedRepository,
            IPersonRepository personRepository, ICarRepository carRepository, IUnitRepository unitRepository, IMapper mapper)
        {
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
            //获取当前用户所拥有的设备
            var devices = await viewAllTargetRepository.GetDevicesByUser(userId);
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
                    name = device.LICENSE,
                    pId = $"unit-{device.UNITID}",
                    id = $"car-{ device.CARID }"
                };
                switch (device.TARGET_TYPE)
                {
                    case "车辆":
                        treeNode.iconSkin = "gray_car";
                        break;
                    case "人员":
                        treeNode.iconSkin = "red_person";
                        break;
                    case "对讲机":
                        treeNode.iconSkin = "blue_phone";
                        break;
                    default:
                        treeNode.iconSkin = "red_person";
                        break;
                }
                listNode.Add(treeNode);
            }
            return listNode.ToArray();
        }

        public async Task<VueTreeSelectModel[]> GetVueDeviceTreeByUser(int userId)
        {
            var allNodes = await GetDeviceTreeByUser(userId);
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
            var exEnties = mapper.Map<List<LastLocatedExEntity>>(lastLocateds);
            return exEnties;
        }

        public async Task<List<ViewAllTargetExEntity>> GetAllDeviceByUser(int userId)
        {
            var viewAllTargetEntities = await viewAllTargetRepository.GetDevicesByUser(userId);
            var exEntities = mapper.Map<List<ViewAllTargetExEntity>>(viewAllTargetEntities);
            return exEntities;
        }

        public async Task<List<NewTrackExEntity>> GetNewTrackList(dynamic queryModel)
        {
            var entities = await newtrackRepository.GetNewtracksByDeviceId(queryModel);
            var exEntities = mapper.Map<List<NewTrackExEntity>>(entities);
            return exEntities;
        }
    }
}
