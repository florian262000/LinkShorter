export default interface UserShortlink {
  Id: string;
  ShortPath: string;
  FullShortUrl: string;
  TargetUrl: string;
  ClickCounter: number;
  TimeStamp: string;
  CreatorId: string;
}
