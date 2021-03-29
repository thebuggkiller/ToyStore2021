﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface ICartService
    {
        void AddCartIntoMember(Cart cart, int MemberID);
        bool CheckCartMember(int MemberID);
        void UpdateQuantityCartMember(int Quantity, int ProductID, int MemberID);
        void AddQuantityProductCartMember(int ProductID, int MemberID);
        void RemoveCart(int ProductID, int MemberID);
        void AddCart(Cart cart);
        List<Cart> GetCart(int MemberID);
        bool CheckProductInCart(int ProductID, int MemberID);
    }
    public class CartService : ICartService
    {
        private readonly UnitOfWork context;
        public CartService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }

        public void AddCart(Cart cart)
        {
            context.CartRepository.Insert(cart);
        }

        public void AddCartIntoMember(Cart cart, int MemberID)
        {
            Cart itemcart = new Cart();
            itemcart.ProductID = cart.ID;
            itemcart.MemberID = MemberID;
            itemcart.Price = cart.Price;
            itemcart.Total = cart.Total;
            itemcart.Image = cart.Image;
            itemcart.Name = cart.Name;
            itemcart.Quantity = cart.Quantity;
            context.CartRepository.Insert(itemcart);
        }

        public void AddQuantityProductCartMember(int ProductID, int MemberID)
        {
            Cart cartUpdate = context.CartRepository.GetAllData().Single(x => x.ProductID == ProductID && x.MemberID == MemberID);
            cartUpdate.Quantity += 1;
            context.CartRepository.Update(cartUpdate);
        }

        public bool CheckCartMember(int MemberID)
        {
            if (context.CartRepository.GetAllData().Where(x => x.MemberID == MemberID).Count() > 0)
                return true;
            return false;
        }

        public bool CheckProductInCart(int ProductID, int MemberID)
        {
            if (context.CartRepository.GetAllData().Where(x => x.MemberID == MemberID && x.ProductID == ProductID).Count() > 0)
                return true;
            return false;
        }

        public List<Cart> GetCart(int MemberID)
        {
            return context.CartRepository.GetAllData().Where(x => x.MemberID == MemberID).ToList();
        }

        public void RemoveCart(int ProductID, int MemberID)
        {
            Cart cart = context.CartRepository.GetAllData().Single(x => x.ProductID == ProductID && x.MemberID == MemberID);
            context.CartRepository.Remove(cart);
        }

        public void UpdateQuantityCartMember(int Quantity, int ProductID, int MemberID)
        {
            Cart cartUpdate = context.CartRepository.GetAllData().Single(x => x.ProductID == ProductID && x.MemberID == MemberID);
            cartUpdate.Quantity = Quantity;
            cartUpdate.Total = cartUpdate.Quantity * cartUpdate.Price;
            context.CartRepository.Update(cartUpdate);
        }
    }
}