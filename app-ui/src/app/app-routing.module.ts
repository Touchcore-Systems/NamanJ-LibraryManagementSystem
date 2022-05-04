import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { ShowBookAdminComponent } from './admin/show-book/show-book.component';
import { ShowBookStudentComponent } from './student/show-book/show-book.component';
import { AuthGuardService } from './guards/auth-guard.service';
import { BooksToApproveComponent } from './admin/books-to-approve/books-to-approve.component';
import { IssueDetailsComponent } from './admin/issue-details/issue-details.component';
import { MyBooksComponent } from './student/my-books/my-books.component';

const routes: Routes = [
  {
    path: "", component: LoginComponent
  },
  {
    path: 'admin/available-books', component: ShowBookAdminComponent, canActivate: [AuthGuardService]
  },
  {
    path: 'admin/approve', component: BooksToApproveComponent, canActivate: [AuthGuardService]
  },
  {
    path: 'admin/issue-details', component: IssueDetailsComponent, canActivate: [AuthGuardService]
  },
  {
    path: 'student/available-books', component: ShowBookStudentComponent, canActivate: [AuthGuardService]
  },
  {
    path: 'student/my-books', component: MyBooksComponent, canActivate: [AuthGuardService]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
