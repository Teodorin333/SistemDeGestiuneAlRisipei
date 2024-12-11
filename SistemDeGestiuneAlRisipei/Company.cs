﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemDeGestiuneAlRisipei
{
    public class Company : AUser
    {
        private static int idCounter = 130000;

        public Company() : base()
        {
            idCounter++;
            this.Id = idCounter;
        }

        public Company(int id, string email, string username, string password, double latitudineAdresa, double longitudineAdresa, string phoneNumber, List<Order> orders) : base(id, email, username, password, latitudineAdresa, longitudineAdresa, phoneNumber, orders)
        {
            idCounter++;
        }

        public Company(string email, string username, string password, string phoneNumber) : base(email, username, password, phoneNumber)
        {
            idCounter++;
            this.Id = idCounter;
        }

        public override void newOrder(Order order)
        {
            Orders.Add(order);
        }

        public override void modifyOrder(Order order)
        {
            int index = this.Orders.IndexOf(order);
            Orders[index] = order;
        }
        public override void deleteOrder(Order order)
        {
            Orders.Remove(order);
        }

        public override void confirmOrder(Order order)
        {
            order.Status = OrderStatusEnum.VERIFIED;
        }
    }
}
