class Convert {
  public static ToRestrictedNumber(value: string | undefined, minimumValue: number): number {
    const result = parseInt(value ?? `${minimumValue}`);

    if (result <= minimumValue) {
      return minimumValue;
    }

    return result;
  }
}

export { Convert };
