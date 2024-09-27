import { computed, inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Cart, CartItem } from '../../shared/models/cart';
import { Product } from '../../shared/models/product';
import { map } from 'rxjs';
import { toObservable } from '@angular/core/rxjs-interop';
import { DeliveryMethod } from '../../shared/models/deliveryMethod';
@Injectable({
  providedIn: 'root'
})
export class CartService {
  baseUrl = environment.apiUrl;
  http = inject(HttpClient);
  cart = signal<Cart | null> (null);
  itemCount = computed(() => {
    return this.cart()?.items.reduce((sum, item) => sum + item.quantity,0);
  })
  itemCount$ = toObservable(this.itemCount);
  selectedDelivery = signal<DeliveryMethod | null>(null);
  total = computed(() => {
    const cart = this.cart();
    const delivery = this.selectedDelivery();
    if (!cart) return null;
    const subTotal = cart.items.reduce((sum,item) => sum + item.quantity*item.price,0);
    const shipping = delivery ? delivery.price : 0;
    const discount = 0;
    return {
      subTotal,
      shipping,
      discount,
      total: subTotal + shipping - discount
    }
  })

  getCart(id: string) {
    return this.http.get<Cart>(this.baseUrl + 'shoppingcart?id=' + id).pipe(
      map(cart => {
        this.cart.set(cart);
        return cart;
      })
    )
  }

  setCart(cart: Cart) {
    return this.http.post<Cart>(this.baseUrl + 'shoppingcart',cart).subscribe({
      next: cart => this.cart.set(cart),
    })
  }

  addItemToCart(item: CartItem | Product, quantity = 1) {
    const cart = this.cart() ?? this.createCart()
    if (this.isProduct(item)) {
      item = this.mapProductToItem(item);
    }
    cart.items = this.addOrUpdate(cart.items, item, quantity);
    this.setCart(cart);
  }


  addOrUpdate(items: CartItem[], item: CartItem, quantity: number): CartItem[] {
    console.log(item);
    const index = items.findIndex(x => x.id === item.id);
    if (index === -1) {
      item.quantity = quantity;
      items.push(item);
    } else {
      items[index].quantity += quantity
    }
    return items;
  }


  private mapProductToItem(item: Product): CartItem {
    return {
      id: item.id,
      name: item.name,
      price: item.price,
      quantity: 0,
      brand: item.brand,
      type: item.type,
      pictureUrl: item.pictureUrl
    }
  }

  private isProduct(item: Product | CartItem): item is Product {
    return (item as Product).id !== undefined;
  }
  private createCart(): Cart  {
    console.log('abc');
    const cart = new Cart();
    localStorage.setItem('cart_id', cart.id);
    return cart;
  }

  removeItemFromCart(productId: number, quantity = 1) {
    const cart = this.cart();
    if (!cart) return;
    const index = cart.items.findIndex( x => x.id === productId);
    if (index !== -1) {
      if (cart.items[index].quantity > quantity) {
        cart.items[index].quantity-=quantity;
      } else {
        cart.items.splice(index,1);
      }
      if (cart.items.length === 0) this.deleteCart();
        else this.setCart(cart);
    }
  }
  deleteCart() {
    this.http.delete(this.baseUrl + 'shoppingcart?id=' + this.cart()?.id).subscribe({
      next: () => {
        localStorage.removeItem('cart_id');
        this.cart.set(null);
      }
    })
  }

}
