import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BorrowRequests } from './borrow-requests';

describe('BorrowRequests', () => {
  let component: BorrowRequests;
  let fixture: ComponentFixture<BorrowRequests>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BorrowRequests]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BorrowRequests);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
