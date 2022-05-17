import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AdminComponent } from './admin/admin.component';
import { AddEditBooksAdminComponent } from './admin/add-edit-books/add-edit-books.component';
import { ShowBooksAdminComponent } from './admin/show-books/show-books.component';

import { MatTableModule } from '@angular/material/table';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from './login/login.component';
import { JwtModule } from '@auth0/angular-jwt';
import { AuthGuardService } from './auth/auth-guard.service';
import { MyBooksComponent } from './student/my-books/my-books.component';
import { ShowBooksStudentComponent } from './student/show-books/show-books.component';
import { BooksToApproveComponent } from './admin/books-to-approve/books-to-approve.component';
import { IssueDetailsComponent } from './admin/issue-details/issue-details.component';

export function tokenGetter(){
  return localStorage.getItem('lms_authenticate');
}

@NgModule({
  declarations: [
    AppComponent,
    AdminComponent,
    AddEditBooksAdminComponent,
    ShowBooksAdminComponent,
    LoginComponent,
    MyBooksComponent,
    ShowBooksStudentComponent,
    BooksToApproveComponent,
    IssueDetailsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MatTableModule,
    BrowserAnimationsModule,
    MatSnackBarModule,
    MatPaginatorModule,
    MatSortModule,
    MatFormFieldModule,
    MatInputModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:7298"],
        // encountered before angular router
        disallowedRoutes: []
      }
    }),
  ],
  providers: [AuthGuardService],
  bootstrap: [AppComponent]
})
export class AppModule { }
