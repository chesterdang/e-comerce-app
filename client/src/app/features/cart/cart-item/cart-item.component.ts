import { Component, inject, input, Pipe } from '@angular/core';
import { CartItem } from '../../../shared/models/cart';
import { RouterLink } from '@angular/router';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { CurrencyPipe } from '@angular/common';
import { pipe } from 'rxjs';
import { CartService } from '../../../core/services/cart.service';

@Component({
  selector: 'app-cart-item',
  standalone: true,
  imports: [
    RouterLink,
    MatButton,
    MatIcon,
    CurrencyPipe,
    
  ],
  templateUrl: './cart-item.component.html',
  styleUrl: './cart-item.component.scss'
})
export class CartItemComponent {
  item = input.required<CartItem>();
  cartService = inject(CartService);
  increaseItem() {
    this.cartService.addItemToCart(this.item());
  }
  decreaseItem() {
    this.cartService.removeItemFromCart(this.item().id);
  }
  removeItem() {
    this.cartService.removeItemFromCart(this.item().id,this.item().quantity);
  }
}
