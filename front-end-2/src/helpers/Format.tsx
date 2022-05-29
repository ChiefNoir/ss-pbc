class Format {
    public static ToDate(value: Date): string {
        const dt = new Date(value);
        return `${dt.getFullYear()}
                / ${("0" + (dt.getMonth() + 1)).slice(-2)}
                / ${("0" + dt.getDate()).slice(-2)}`;
    }
}

export { Format };