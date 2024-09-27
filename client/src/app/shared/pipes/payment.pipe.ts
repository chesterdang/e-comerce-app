import { UpperCasePipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';
import { ConfirmationToken} from '@stripe/stripe-js'
import { PaymentSummary } from '../models/order';
import { P } from '@angular/cdk/keycodes';

@Pipe({
  name: 'payment',
  standalone: true
})
export class PaymentPipe implements PipeTransform {

  transform(value?: ConfirmationToken['payment_method_preview'] | PaymentSummary, ...args: unknown[]): unknown {
    if (value && 'card' in value) {
      if (value?.card)
      return `${value.card.brand.toUpperCase()} **** **** **** ${value.card.last4}, Exp: ${value.card.exp_month}/${value.card.exp_year}`;
      else  return 'Missing payment detail';
    } else if (value && 'last4' in value){
      const {brand,last4, expMonth, expYear}= value as PaymentSummary;
      return `${brand.toUpperCase()} **** **** **** ${last4}, Exp: ${expMonth}/${expYear}`;
    } else {
      return 'Unknown payment method';
    }
  }

}
