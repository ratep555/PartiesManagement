import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BirthdayPackage } from 'src/app/shared/models/birthdays/birthdaypackage';
import { BirthdayPackagesService } from '../birthday-packages.service';

@Component({
  selector: 'app-birthday-package-info',
  templateUrl: './birthday-package-info.component.html',
  styleUrls: ['./birthday-package-info.component.css']
})
export class BirthdayPackageInfoComponent implements OnInit {
  birthdayPackage: BirthdayPackage;

  constructor(private birthdayPackagesService: BirthdayPackagesService,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadBirthdayPackage();
  }

  loadBirthdayPackage() {
    return this.birthdayPackagesService.getBirthdayPackage(+this.activatedRoute.snapshot.paramMap.get('id'))
    .subscribe(response => {
    this.birthdayPackage = response;
    }, error => {
    console.log(error);
    });
    }


}
