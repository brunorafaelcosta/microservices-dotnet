export class DocumentUtils {
    public static GetDocumentOrigin() {
        if (!document.location.origin) {
            return document.location.protocol 
                + '//' 
                + document.location.hostname 
                + (document.location.port ? ':' + document.location.port : '');
        }
    
        return document.location.origin;
    }
}
