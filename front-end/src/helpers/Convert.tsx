class Convert {
  public static ToRestrictedNumber(value: string | undefined | null, minimumValue: number): number {
    if (!value) {
      return minimumValue;
    }

    const result = parseInt(value ?? `${minimumValue}`);

    if (result <= minimumValue) {
      return minimumValue;
    }

    return result;
  }
}

export { Convert };
