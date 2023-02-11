CREATE TABLE VECTORS (
    Id      TEXT    PRIMARY KEY,
    MemberKey TEXT,
    InternalKey TEXT,
    Vector BLOB,
    Image TEXT,
    ImageType NUMBER);
 
CREATE INDEX vec_mkey_index ON VECTORS (MemberKey);   
CREATE INDEX vec_mid_index ON VECTORS (InternalKey);   
