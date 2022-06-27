import { configureStore } from "@reduxjs/toolkit";
import { identitySlice } from "./slices/identitySlice";
import fingerprintSlice from "./slices/fingerprintSlice";

const store = configureStore({
  reducer: {
    identity: identitySlice.reducer,
    fingerprint: fingerprintSlice
  }
});

export { store };
export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
