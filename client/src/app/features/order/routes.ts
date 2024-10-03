import { Route } from "@angular/router";
import { authGuard } from "../../core/guard/auth.guard";
import { emptyGuard } from "../../core/guard/empty.guard";
import { orderCompleteGuard } from "../../core/guard/order-complete.guard";
import { CheckoutSuccessComponent } from "../checkout/checkout-success/checkout-success.component";
import { CheckoutComponent } from "../checkout/checkout.component";

export const orderRoutes: Route[] = [
    {path: 'checkout', component: CheckoutComponent, canActivate: [authGuard,emptyGuard]},
    {path: 'checkout/success', component: CheckoutSuccessComponent, canActivate: [authGuard, orderCompleteGuard]},
]