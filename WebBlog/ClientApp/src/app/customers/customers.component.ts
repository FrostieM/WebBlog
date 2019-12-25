import {Component, Inject, OnInit} from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-customers',
  templateUrl: './customers.component.html',
  styleUrls: []
})
export class CustomersComponent implements OnInit  {
  customers: any;

  constructor(private http: HttpClient, @Inject("BASE_URL") private baseUrl: string) { }

  ngOnInit() {

    this.http.get(this.baseUrl + "api/customers", {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
      this.customers = response;
    }, err => {
      console.log(err)
    });
  }
}
