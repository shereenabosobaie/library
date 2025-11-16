import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Singup } from './singup';

describe('Singup', () => {
  let component: Singup;
  let fixture: ComponentFixture<Singup>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [Singup]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Singup);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
