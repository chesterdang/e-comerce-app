import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Product } from '../../shared/models/product';
import { ShopParams} from '../../shared/models/shopParams';
import { Pagination } from '../../shared/models/pagination';
import { environment } from '../../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseURL= environment.apiUrl;
  private http=inject(HttpClient);
  types: string[] = [];
  brands: string[] = [];
  getProducts(shopParams: ShopParams) {
    // return this.http.get<Product[]>(this.baseURL + 'products?pageSize=20')
    let params = new HttpParams();
    if (shopParams.brands.length >0) {
      params = params.append('brands',shopParams.brands.join(',')); 
    }
    if (shopParams.types.length>0) {
      params = params.append('types',shopParams.types.join(','));
    }
    if (shopParams.sort) {
      params = params.append('sort',shopParams.sort);
    }
    if (shopParams.search) {
      params = params.append('search',shopParams.search);
    }
    params = params.append('pageIndex',shopParams.pageNumber);
    params = params.append('pageSize',shopParams.pageSize);
    return this.http.get<Pagination<Product>>(this.baseURL + 'products/', {params})
  }
  getProduct(id: number) {
    return this.http.get<Product>(this.baseURL+'products/' + id);
  }
  getBrands() {
    if (this.brands.length>0) return;
    return this.http.get<string[]>(this.baseURL + 'products/brands').subscribe({
      next: response => this.brands = response
    })
  }
  getTypes() {
    if (this.types.length>0) return;
    return this.http.get<string[]>(this.baseURL + 'products/types').subscribe({
      next: response => this.types = response
    })
  }
}
