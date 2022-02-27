import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Birthday } from 'src/app/shared/models/birthdays/birthday';
import { MyParams } from 'src/app/shared/models/myparams';
import { BirthdayPackagesService } from '../birthday-packages.service';

@Component({
  selector: 'app-birthdays-list',
  templateUrl: './birthdays-list.component.html',
  styleUrls: ['./birthdays-list.component.css']
})
export class BirthdaysListComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  birthdays: Birthday[];
  myParams: MyParams;
  totalCount: number;

  sortOptions = [
    {name: 'All', value: 'all'},
    {name: 'Pending', value: 'pending'},
    {name: 'Approved', value: 'approved'}
  ];

  constructor(private birthdayPackageService: BirthdayPackagesService) {
    this.myParams = this.birthdayPackageService.getMyParams();
   }

  ngOnInit(): void {
    this.getBirthdays();
  }

  getBirthdays() {
    this.birthdayPackageService.getBirthdays().subscribe(response => {
      this.birthdays = response.data;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    });
  }

  onPageChanged(event: any) {
    const params = this.birthdayPackageService.getMyParams();
    if (params.page !== event) {
      params.page = event;
      this.birthdayPackageService.setMyParams(params);
      this.getBirthdays();
    }
  }

  onSortSelected(sort: string) {
    const params = this.birthdayPackageService.getMyParams();
    params.sort = sort;
    params.page = 1;
    this.birthdayPackageService.setMyParams(params);
    this.getBirthdays();
  }

  onSearch() {
    const params = this.birthdayPackageService.getMyParams();
    params.query = this.searchTerm.nativeElement.value;
    params.page = 1;
    this.birthdayPackageService.setMyParams(params);
    this.getBirthdays();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.myParams = new MyParams();
    this.birthdayPackageService.setMyParams(this.myParams);
    this.getBirthdays();
  }
}

