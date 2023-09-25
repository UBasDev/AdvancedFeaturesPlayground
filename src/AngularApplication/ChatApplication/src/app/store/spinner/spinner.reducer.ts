import { createReducer, on } from "@ngrx/store";
import { SpinnerActions } from "./spinner.actionts";

export interface ISpinnerStateInitialState {
    isOpen: boolean
}
export const SpinnerStateInitialState1: ISpinnerStateInitialState = {
    isOpen: false
  };
  export const SpinnerStateReducer = createReducer(
    SpinnerStateInitialState1,
    on(SpinnerActions.toggleSpinner, (_state, payload) => {
        return {
          isOpen: payload.isOpen
        };
      }),
  )