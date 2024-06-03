import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewRecentPostsComponent } from './view-recent-posts.component';

describe('ViewPostsComponent', () => {
  let component: ViewRecentPostsComponent;
  let fixture: ComponentFixture<ViewRecentPostsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ViewRecentPostsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ViewRecentPostsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
