import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BooksToApproveComponent } from './books-to-approve.component';

describe('BooksToApproveComponent', () => {
  let component: BooksToApproveComponent;
  let fixture: ComponentFixture<BooksToApproveComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BooksToApproveComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BooksToApproveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
