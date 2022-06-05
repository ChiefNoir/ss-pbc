import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { Identity } from "../services";
import { WebStorage } from "./sessionStorage";

export const identitySlice = createSlice({
  name: "identity",
  initialState: {
    value: WebStorage.LoadIdentity()
  },
  reducers: {
    deleteIdentity: (state) => {
      state.value = null;
      WebStorage.SaveIdentity(null);
    },
    saveIdentity: (state, action: PayloadAction<Identity>) => {
      state.value = action.payload;
      WebStorage.SaveIdentity(action.payload);
    }
  }
});

export const { saveIdentity, deleteIdentity } = identitySlice.actions;

export default identitySlice.reducer;
