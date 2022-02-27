import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Birthday, BirthdayByIdForEdit, BirthdayCreate, BirthdayEdit } from '../shared/models/birthdays/birthday';
import { BirthdayPackage, BirthdayPackageCreateEdit, BirthdayPackageCreateEditClass, BirthdayPackagePutGet } from '../shared/models/birthdays/birthdaypackage';
import { ServiceIncluded } from '../shared/models/birthdays/serviceincluded';
import { ItemPutGet } from '../shared/models/item';
import { MyParams } from '../shared/models/myparams';
import { IPaginationForBirthdayPackages, PaginationForBirthdayPackages, PaginationForBirthdays } from '../shared/models/pagination';

@Injectable({
  providedIn: 'root'
})
export class BirthdayPackagesService {
  baseUrl = environment.apiUrl;
  birthdayPackages: BirthdayPackage[] = [];
  pagination = new PaginationForBirthdayPackages();
  pagination1 = new PaginationForBirthdays();
  myParams = new MyParams();
  itemCache = new Map();
  servicesIncluded: ServiceIncluded[] = [];
  formData: BirthdayEdit = new BirthdayEdit();
  formData1: BirthdayPackageCreateEditClass = new BirthdayPackageCreateEditClass();

  constructor(private http: HttpClient) { }

  getBirthdayPackages() {
     let params = new HttpParams();
     if (this.myParams.query) {
       params = params.append('query', this.myParams.query);
     }
     params = params.append('page', this.myParams.page.toString());
     params = params.append('pageCount', this.myParams.pageCount.toString());
     return this.http.get<IPaginationForBirthdayPackages>
     (this.baseUrl + 'birthdays/birthdaypackages', { observe: 'response', params })
       .pipe(
         map(response => {
           this.pagination = response.body;
           return this.pagination;
         })
       );
   }

  getBirthdays() {
     let params = new HttpParams();
     if (this.myParams.query) {
       params = params.append('query', this.myParams.query);
     }
     params = params.append('sort', this.myParams.sort);
     params = params.append('page', this.myParams.page.toString());
     params = params.append('pageCount', this.myParams.pageCount.toString());
     return this.http.get<PaginationForBirthdays>
     (this.baseUrl + 'birthdays', { observe: 'response', params })
       .pipe(
         map(response => {
           this.pagination1 = response.body;
           return this.pagination1;
         })
       );
   }

   setMyParams(params: MyParams) {
    this.myParams = params;
  }

  getMyParams() {
    return this.myParams;
  }

  getBirthdayPackage(id: number) {
    return this.http.get<BirthdayPackage>(this.baseUrl + 'birthdays/birthdaypackages/' + id);
  }

  getBirthdayPackage1(id: number) {
    return this.http.get<BirthdayPackageCreateEditClass>(this.baseUrl + 'birthdays/birthdaypackages/' + id);
  }

  createBirthday(birthday: BirthdayCreate) {
    return this.http.post(this.baseUrl + 'birthdays', birthday);
  }

  getBirthdayById(id: number) {
    return this.http.get<BirthdayByIdForEdit>(this.baseUrl + 'birthdays/' + id);
  }

  updateBirthday(formData){
    return this.http.put(this.baseUrl + 'birthdays/' + formData.id, formData);
  }

  updateDiscount1(formData){
    return this.http.put(this.baseUrl + 'items/discountput1/' + formData.id, formData);
  }

  putGetBirthdayPackage(id: number): Observable<BirthdayPackagePutGet>{
    return this.http.get<BirthdayPackagePutGet>(this.baseUrl + 'birthdays/birthdaypackages/putget/' + id);
  }

  createBirthdayPackage(birthdayPackage: BirthdayPackageCreateEdit) {
    const formData = this.BuildFormData(birthdayPackage);
    return this.http.post(this.baseUrl + 'birthdays/birthdaypackages', formData);
  }

  updateBirthdayPackage(id: number, birthdayPackage: BirthdayPackageCreateEdit){
    const formData = this.BuildFormData(birthdayPackage);
    return this.http.put(this.baseUrl + 'birthdays/birthdaypackages/' + id, formData);
  }

  updateBirthdayPackage1(formData1){
    const formData = this.BuildFormData(formData1);
    return this.http.put(this.baseUrl + 'birthdays/birthdaypackages/' + formData1.id, formData1);
  }

  getLocations() {
    return this.http.get<Location[]>(this.baseUrl + 'birthdays/birthdaypackages/locations');
  }

  getBPackages() {
    return this.http.get<BirthdayPackage[]>(this.baseUrl + 'birthdays/birthdaypackages/list');
  }

  getAllServices() {
    return this.http.get<ServiceIncluded[]>(this.baseUrl + 'birthdays/birthdaypackages/services');
  }

  private BuildFormData(birthdayPackage: BirthdayPackageCreateEdit): FormData {
    const formData = new FormData();
    formData.append('price', JSON.stringify(birthdayPackage.price));
    formData.append('numberOfParticipants', JSON.stringify(birthdayPackage.numberOfParticipants));
    formData.append('additionalBillingPerParticipant', JSON.stringify(birthdayPackage.additionalBillingPerParticipant));
    formData.append('duration', JSON.stringify(birthdayPackage.duration));
    if (birthdayPackage.id) {
      formData.append('id', JSON.stringify(birthdayPackage.id));
    }
    if (birthdayPackage.packageName){
    formData.append('packageName', birthdayPackage.packageName);
    }
    if (birthdayPackage.description){
    formData.append('description', birthdayPackage.description);
    }
    if (birthdayPackage.picture){
      formData.append('picture', birthdayPackage.picture);
    }
    if (birthdayPackage.discountsIds) {
      formData.append('discountsIds', JSON.stringify(birthdayPackage.discountsIds));
    }
    if (birthdayPackage.servicesIds) {
      formData.append('servicesIds', JSON.stringify(birthdayPackage.servicesIds));
    }
    return formData;
  }


}











