import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BorrowReq } from './borrow-req';

describe('BorrowReq', () => {
  let component: BorrowReq;
  let fixture: ComponentFixture<BorrowReq>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BorrowReq]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BorrowReq);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
