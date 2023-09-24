import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChatHomepageComponent } from './chat-homepage.component';

describe('ChatHomepageComponent', () => {
  let component: ChatHomepageComponent;
  let fixture: ComponentFixture<ChatHomepageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ChatHomepageComponent]
    });
    fixture = TestBed.createComponent(ChatHomepageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
