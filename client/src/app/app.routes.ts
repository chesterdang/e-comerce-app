import { Routes } from '@angular/router';
import {HomeComponent} from './features/home/home.component';
import {ShopComponent} from './features/shop/shop.component';
import {ProductDetailsComponent} from './features/shop/product-details/product-details.component';
import {TestErrorComponent} from './features/test-error/test-error.component';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';
import { ServerErrorComponent } from './shared/components/server-error/server-error.component';
import { CartComponent } from './features/cart/cart.component';
import { CheckoutComponent } from './features/checkout/checkout.component';
import { LoginComponent } from './features/account/login/login.component';
import { RegisterComponent } from './features/account/register/register.component';
import { authGuard } from './core/guard/auth.guard';
import { emptyGuard } from './core/guard/empty.guard';
import { CheckoutSuccessComponent } from './features/checkout/checkout-success/checkout-success.component';
import { OrderComponent } from './features/order/order.component';
import { OrderDetailComponent } from './features/order/order-detail/order-detail.component';
import { orderCompleteGuard } from './core/guard/order-complete.guard';
export const routes: Routes = [
    {path: '', component: HomeComponent},
    {path: 'shop', component: ShopComponent},
    {path: 'shop/:id', component: ProductDetailsComponent},
    {path: 'account/login', component: LoginComponent},
    {path: 'account/register', component: RegisterComponent},
    {path: 'cart', component: CartComponent},
    {path: 'checkout', component: CheckoutComponent, canActivate: [authGuard,emptyGuard]},
    {path: 'checkout/success', component: CheckoutSuccessComponent, canActivate: [authGuard, orderCompleteGuard]},
    {path: 'order', component: OrderComponent, canActivate: [authGuard]},
    {path: 'order/:id', component: OrderDetailComponent, canActivate: [authGuard]},
    {path: 'test-error', component: TestErrorComponent},
    {path: 'not-found', component: NotFoundComponent},
    {path: 'server-error', component: ServerErrorComponent},
    {path: '**', redirectTo: ''},
];
