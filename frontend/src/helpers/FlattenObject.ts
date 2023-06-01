export function FlattenObject(obj: any) {
  const result: any = {};
  for (const key in obj) {
    result[key] = obj[key];
  }
  return result;
}
