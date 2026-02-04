import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DrugSearchComponent } from './drug-search.component';

describe('DrugSearchComponent', () => {
  let component: DrugSearchComponent;
  let fixture: ComponentFixture<DrugSearchComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [DrugSearchComponent]
    });
    fixture = TestBed.createComponent(DrugSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
