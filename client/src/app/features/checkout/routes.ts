import { Route } from "@angular/router"
import { CheckoutComponent } from "./checkout.component"
import { CheckoutSuccessComponent } from "./checkout-success/checkout-success.component"
import { authGuard } from "../../core/guard/auth.guard"
import { emptyGuard } from "../../core/guard/empty.guard"
import { orderCompleteGuard } from "../../core/guard/order-complete.guard"

export const checkoutRoutes: Route[] = [
    {path: '', component: CheckoutComponent, canActivate: [authGuard,emptyGuard]},
    {path: 'success', component: CheckoutSuccessComponent, canActivate: [authGuard, orderCompleteGuard]},
]