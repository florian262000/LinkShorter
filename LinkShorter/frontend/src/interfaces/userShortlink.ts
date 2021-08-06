export default interface UserShortlink {
  Id: string;
  ShortPath: string;
  TargetUrl: string;
  ClickCounter: number;
  TimeStamp: string;
  CreatorId: string;
}
