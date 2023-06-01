export function ConvertToJSDateTime(serverDate: Date | string) {
  return typeof serverDate === 'string' ? new Date(serverDate) : serverDate;
}

export function DateStringFormat(date: Date) {
  const jsDate = ConvertToJSDateTime(date);
  const currentYear = jsDate.getFullYear();
  const currentMonth = jsDate.getMonth() + 1;
  const currentDate = jsDate.getDate();
  return `${currentYear}-${
    currentMonth < 10 ? `0${currentMonth}` : currentMonth
  }-${currentDate < 10 ? `0${currentDate}` : currentDate}`;
}
