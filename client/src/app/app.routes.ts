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
import { AdminComponent } from './features/admin/admin.component';
import { adminGuard } from './core/guard/admin.guard';
export const routes: Routes = [
    {path: '', component: HomeComponent},
    {path: 'shop', component: ShopComponent},
    {path: 'shop/:id', component: ProductDetailsComponent},
    {path: 'account', loadChildren: () => import('./features/account/routes')
        .then(r => r.accountRoutes)},
    {path: 'cart', component: CartComponent},
    {path: 'checkout', loadChildren: () => import('./features/checkout/routes')
        .then( r => r.checkoutRoutes) },
    {path: 'order', loadChildren: () => import('./features/order/routes')
        .then(r => r.orderRoutes)},
    {path: 'test-error', component: TestErrorComponent},
    {path: 'not-found', component: NotFoundComponent},
    {path: 'server-error', component: ServerErrorComponent},
    {path: '**', redirectTo: ''},
];
