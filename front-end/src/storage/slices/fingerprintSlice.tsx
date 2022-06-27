import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import FingerprintJS from "@fingerprintjs/fingerprintjs";

const fpPromise = FingerprintJS.load({
  monitoring: false
});

export const fetchFingerprint = createAsyncThunk("fingerprint/fetchFingerprint", async() => {
  const fp = await fpPromise;
  const result = await fp.get();
  console.log("fetchUsers|visitorId " + result.visitorId);
  return result.visitorId;
});

const fingerprintSlice = createSlice({
  name: "fingerprint",
  initialState: {
    value: "none"
  },
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchFingerprint.fulfilled, (state, action) => {
        state.value = action.payload;
      });
  }
});

export default fingerprintSlice.reducer;
