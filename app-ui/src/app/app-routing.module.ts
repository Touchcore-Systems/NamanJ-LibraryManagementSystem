import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BooksToApproveComponent } from './admin/books-to-approve/books-to-approve.component';
import { IssueDetailsComponent } from './admin/issue-details/issue-details.component';
import { ShowBooksAdminComponent } from './admin/show-books/show-books.component';
import { AuthGuardService } from './auth/auth-guard.service';
import { LoginComponent } from './login/login.component';
import { MyBooksComponent } from './student/my-books/my-books.component';
import { ShowBooksStudentComponent } from './student/show-books/show-books.component';

const routes: Routes = [
  {
    path:"", component: LoginComponent
  },
  {
    path:"admin/available-books", component: ShowBooksAdminComponent, canActivate: [AuthGuardService]
  },
  {
    path:"admin/books-to-approve", component: BooksToApproveComponent, canActivate: [AuthGuardService]
  },
  {
    path:"admin/issue-details", component: IssueDetailsComponent, canActivate: [AuthGuardService]
  },
  {
    path:"student/available-books", component: ShowBooksStudentComponent, canActivate: [AuthGuardService]
  },
  {
    path:"student/my-books", component: MyBooksComponent, canActivate: [AuthGuardService]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
