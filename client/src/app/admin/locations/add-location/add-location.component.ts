import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/account/account.service';
import { CoordinatesMap } from 'src/app/shared/models/coordinate';
import { Location1 } from 'src/app/shared/models/location';
import { LocationsService } from '../locations.service';

@Component({
  selector: 'app-add-location',
  templateUrl: './add-location.component.html',
  styleUrls: ['./add-location.component.css']
})
export class AddLocationComponent implements OnInit {
  locationForm: FormGroup;
  countryList = [];
  initialCoordinates: CoordinatesMap[] = [];
  model: Location1;

  constructor(public locationsService: LocationsService,
              private accountService: AccountService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.accountService.getCountries()
    .subscribe(res => this.countryList = res as []);

    this.createLocationForm();
  }

  createLocationForm() {
    this.locationForm = this.fb.group({
      street: [null, [Validators.required,
        Validators.minLength(2), Validators.maxLength(40)]],
      city: [null, [Validators.required,
        Validators.minLength(2), Validators.maxLength(40)]],
      email: [null, [Validators.required,
        Validators.minLength(3), Validators.maxLength(60)]],
      phone: [null, [Validators.required,
        Validators.minLength(3), Validators.maxLength(60)]],
      workingHours: [null, [Validators.required,
        Validators.minLength(3), Validators.maxLength(60)]],
      description: ['', [Validators.required,
        Validators.minLength(10), Validators.maxLength(2000)]],
      longitude: ['', [Validators.required]],
      latitude: ['', [Validators.required]],
      countryId: [null]
    });
  }

  onSubmit() {
    this.locationsService.createLocation(this.locationForm.value).subscribe(() => {
      this.router.navigateByUrl('locations');
    },
    error => {
      console.log(error);
    });
  }

  onSelectedLocation(coordinates: CoordinatesMap) {
    this.locationForm.patchValue(coordinates);
 }

}
