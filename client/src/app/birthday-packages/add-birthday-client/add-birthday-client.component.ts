import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BirthdayPackagesService } from '../birthday-packages.service';

@Component({
  selector: 'app-add-birthday-client',
  templateUrl: './add-birthday-client.component.html',
  styleUrls: ['./add-birthday-client.component.css']
})
export class AddBirthdayClientComponent implements OnInit {
  birthdayForm: FormGroup;
  locationList = [];
  birthdayPackageList = [];

  constructor(public birthdayPackagesService: BirthdayPackagesService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.birthdayPackagesService.getLocations()
    .subscribe(res => this.locationList = res as []);

    this.birthdayPackagesService.getBPackages()
    .subscribe(res => this.birthdayPackageList = res as []);

    this.createBirthdayForm();
  }

  createBirthdayForm() {
    this.birthdayForm = this.fb.group({
      clientName: [null, [Validators.required]],
      birthdayGirlBoyName: [null, [Validators.required]],
      contactEmail: [null, [Validators.required]],
      contactPhone: [null, [Validators.required]],
      numberOfGuests: ['', [Validators.required]],
      birthdayNo: ['', [Validators.required]],
      startDateAndTime: ['', Validators.required],
      location1Id: [null],
      birthdayPackageId: [null]
    });
  }

  onSubmit() {
    this.birthdayPackagesService.createBirthday(this.birthdayForm.value).subscribe(() => {
      this.router.navigateByUrl('birthdaypackages');
    },
    error => {
      console.log(error);
    });
  }

}
