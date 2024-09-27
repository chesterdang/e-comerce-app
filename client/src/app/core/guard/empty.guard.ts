import { CanActivateFn, Router } from '@angular/router';
import { CartService } from '../services/cart.service';
import { inject } from '@angular/core';
import { map, of } from 'rxjs';
import { SnackbarService } from '../services/snackbar.service';

export const emptyGuard: CanActivateFn = (route, state) => {
  const cartService = inject(CartService);
  const router = inject(Router);
  const snack = inject(SnackbarService);
  if (cartService.cart() === null ) {
    router.navigateByUrl('/cart');
    snack.error("Your cart is empty");
    return false;
  }
  return true;
};
