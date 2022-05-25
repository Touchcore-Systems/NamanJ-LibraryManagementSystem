import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { NgForm, Validators, FormBuilder } from '@angular/forms';
import { SharedService } from '../shared.service';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  invalidLogin: boolean = false;
  diffPass: boolean = false;

  constructor(private router: Router, private service: SharedService) { }


  ngOnInit(): void { }

  login(form: NgForm) {
    const credentials = {
      'uName': form.value.username,
      'uPass': form.value.password,
      'uRole': form.value.role
    }

    this.service.loginUser(credentials).subscribe(
      res => {
        const token = (<any>res).token;
        localStorage.setItem("lms_authenticate", token); // session storage can also be used
        this.invalidLogin = false;
        if (credentials.uRole == "admin") {
          this.router.navigate(["admin/available-books"]);
        } else {
          this.router.navigate(["student/available-books"]);
        }
        this.service.SnackBarMessage("Login Successful!", "Dismiss");
      }, err => {
        this.invalidLogin = true;
        this.service.SnackBarMessage(err.message, "Dismiss");
      }
    )
  }

  register(form: NgForm) {
    if (form.value.password == form.value.con_password) {
      const details = {
        'uName': form.value.username,
        'uPass': form.value.password,
        'uRole': 'student'
      };

      this.service.registerUser(details).subscribe((res: any) => {
        this.service.SnackBarMessage(res.toString(), "Dismiss");
      });
    }
    else {
      this.diffPass = true;
    }
  }
}
