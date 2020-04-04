import { StringExtensions } from './extesions';

export class UrlUtils {

    public static UrlBase64Decode(url: string): string {
        let output = url.replace('-', '+').replace('_', '/');
        switch (output.length % 4) {
            case 0:
                break;
            case 2:
                output += '==';
                break;
            case 3:
                output += '=';
                break;
            default:
                throw 'Illegal base64url string!';
        }

        return window.atob(output);
    }

    public static MergeUrlPaths(base: string, ...paths: string[]): string {
        let url: string = UrlUtils.TrimLastSlashFromUrl(base);

        url += paths.map((path, i) => {
            if (i === 0) {
              return path.trim().replace(/[\/]*$/g, '')
            } else {
              return path.trim().replace(/(^[\/]*|[\/]*$)/g, '')
            }
          }).filter(x=>x.length).join('/');
        
        return url;
    }

    public static TrimLastSlashFromUrl(url: string) {
        if (StringExtensions.isNullOrWhitespace(url)) {
            return null;
        } else if (url[url.length - 1] == '/') {
            var trimmedUrl = url.substring(0, url.length - 1);
            return trimmedUrl;
        }
    }
}
