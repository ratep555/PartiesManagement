import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { Category } from 'src/app/shared/models/category';
import { Discount, DiscountCreateEdit, DiscountEditClass, DiscountPutGet } from 'src/app/shared/models/discount';
import { Item } from 'src/app/shared/models/item';
import { Manufacturer1 } from 'src/app/shared/models/manufacturer';
import { UserParams } from 'src/app/shared/models/myparams';
import { IPaginationForDiscounts } from 'src/app/shared/models/pagination';
import { Tag } from 'src/app/shared/models/tag';
import { User } from 'src/app/shared/models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DiscountsService {
  baseUrl = environment.apiUrl;
  user: User;
  userParams: UserParams;
  formData: DiscountEditClass = new DiscountEditClass();


  constructor(private http: HttpClient,
              private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take (1)).subscribe(user => {
      this.user = user;
      this.userParams = new UserParams(user);
});
}

  getUserParams() {
    return this.userParams;
  }

  setUserParams(params: UserParams) {
    this.userParams = params;
  }

  resetUserParams() {
    this.userParams = new UserParams(this.user);
    return this.userParams;
  }

  getDiscounts(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<IPaginationForDiscounts>(this.baseUrl + 'items/discountlist', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getDiscountById(id: number) {
    return this.http.get<Discount>(this.baseUrl + 'items/discount/' + id);
  }

  createDiscount(discount: DiscountCreateEdit) {
    return this.http.post(this.baseUrl + 'items/discountpost', discount);
  }

  updateDiscount(id: number, discount: DiscountCreateEdit){
    return this.http.put(this.baseUrl + 'items/discountput' + id, discount);
  }

  updateDiscount1(formData){
    return this.http.put(this.baseUrl + 'items/discountput1/' + formData.id, formData);
  }

  putGetDiscount(id: number): Observable<DiscountPutGet>{
    return this.http.get<DiscountPutGet>(this.baseUrl + 'items/putget1discount1/' + id);
  }

  getAllItems() {
    return this.http.get<Item[]>(this.baseUrl + 'items/discounts/items');
  }

  getAllCategories() {
    return this.http.get<Category[]>(this.baseUrl + 'items/discounts/categories');
  }

  getAllManufacturers() {
    return this.http.get<Manufacturer1[]>(this.baseUrl + 'items/discounts/manufacturers');
  }

  deleteDiscount(id: number) {
    return this.http.delete(this.baseUrl + 'items/discountdelete/' + id);
  }

}






