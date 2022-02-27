import { Component, Input, OnInit } from '@angular/core';
import { BirthdayPackage } from 'src/app/shared/models/birthdays/birthdaypackage';

@Component({
  selector: 'app-birthday-package',
  templateUrl: './birthday-package.component.html',
  styleUrls: ['./birthday-package.component.css']
})
export class BirthdayPackageComponent implements OnInit {
  @Input() birthdayPackage: BirthdayPackage;

  constructor() { }

  ngOnInit(): void {
  }

}
