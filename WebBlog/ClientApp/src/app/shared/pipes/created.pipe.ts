import {Pipe, PipeTransform} from "@angular/core";

@Pipe({
  name: "created"
})
export class CreatedPipe implements PipeTransform{
  transform(value: string, maxDayLeft = 30): string {
    let postDate = Date.parse(value);
    let today = Date.now();

    const oneDay = 24 * 60 * 60 * 1000;

    let daysLeft = Math.round(Math.abs((postDate - today) / oneDay));

    if (daysLeft == 0) return "today";
    if (daysLeft == 1) return "yesterday";
    if (daysLeft <= maxDayLeft) return daysLeft + " days ago";

    let date = new Date(postDate);
    return date.getFullYear() + "/" + (date.getMonth() + 1) + "/" + date.getDay();
  }
}
