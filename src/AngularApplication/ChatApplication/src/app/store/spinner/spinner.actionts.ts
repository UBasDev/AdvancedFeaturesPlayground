import { createActionGroup, props } from "@ngrx/store";

export const SpinnerActions = createActionGroup({
    source: "SpinnerState",
    events: {
      "Toggle Spinner": props<{ isOpen: boolean }>(),
    },
  });