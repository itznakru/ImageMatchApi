CREATE TABLE VECTORS (
    Id      TEXT    PRIMARY KEY,
    MemberKey TEXT,
    InternalKey TEXT,
    Vector TEXT,
    Image TEXT,
    ImageType NUMBER,
    CreateDateTime NUMBER);
 
CREATE INDEX vec_mkey_index ON VECTORS (MemberKey);   
CREATE INDEX vec_mid_index ON VECTORS (InternalKey);   
