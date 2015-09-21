﻿using BankSwitch.Core.Entities;
using BankSwitch.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BankSwitch.Logic
{
    public class RouteManager
    {
        private RouteDAO _db;
        public RouteManager(RouteDAO db)
        {
            _db = db;
        }
        public RouteManager()
        {
            _db = new RouteDAO();
        }
        public bool CreateRoute(Route model)
        {
            try
            {
                var route = _db.Get<Route>().FirstOrDefault(x => x.Name == model.Name && x.CardPAN == model.CardPAN);
                if (route != null)
                {
                    throw new Exception("Route with this Card PAN already exist");
                }
                else
                {
                    return _db.Add(new Route
                    {
                        SinkNode = model.SinkNode,
                        CardPAN = model.CardPAN,
                        Name = model.Name,
                        Description = model.Description
                    });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool EditRoute(Route model)
        {
            bool result = false;
            try
            {
                var route = _db.Get<Route>().Where(x => x.Name == model.Name && x.CardPAN == model.CardPAN).FirstOrDefault();
                if (route != null)
                {
                    route.Name = model.Name;
                    route.CardPAN = model.CardPAN;
                    route.Description = model.Description;
                    route.SinkNode = model.SinkNode;
                    result = _db.Update(route);
                    _db.Commit();
                }
                return result;
            }
            catch (Exception)
            {
                _db.Rollback();
                throw;
            }
        }

        public IList<Route> GetAllRoute()
        {
            try
            {
                var route = _db.GetAllRoute();
                return route;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<Route> RetreiveRouteByName(string name)
        {
            return _db.RetrieveByName(name);
        }
        public IList<Route> Search(string queryString, int pageIndex, int pageSize, out int totalCount)
        {
            return _db.SearchRoute(queryString, pageIndex, pageSize, out totalCount);
        }

        public Route GetRouteByCardPan(string theCardPan)
        {
            var route = _db.Get<Route>().FirstOrDefault(x => x.CardPAN==theCardPan);
            return route;
        }
    }
}