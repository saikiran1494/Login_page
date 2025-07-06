-- FitnessTracker Stored Procedures
-- PL/SQL procedures for business logic

-- 1. User Authentication Procedures

-- Procedure to register a new user
CREATE OR REPLACE PROCEDURE RegisterUser(
    p_username IN VARCHAR2,
    p_email IN VARCHAR2,
    p_password IN VARCHAR2,
    p_mobile IN VARCHAR2,
    p_fullname IN VARCHAR2,
    p_userid OUT NUMBER
)
AS
BEGIN
    INSERT INTO Users (Username, Email, Password, MobileNumber, FullName)
    VALUES (p_username, p_email, p_password, p_mobile, p_fullname)
    RETURNING UserId INTO p_userid;
    
    COMMIT;
EXCEPTION
    WHEN DUP_VAL_ON_INDEX THEN
        RAISE_APPLICATION_ERROR(-20001, 'Username, email, or mobile number already exists');
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20002, 'Error registering user: ' || SQLERRM);
END RegisterUser;
/

-- Procedure to validate user login
CREATE OR REPLACE PROCEDURE ValidateLogin(
    p_username IN VARCHAR2,
    p_password IN VARCHAR2,
    p_userid OUT NUMBER,
    p_fullname OUT VARCHAR2,
    p_mobile OUT VARCHAR2
)
AS
BEGIN
    SELECT UserId, FullName, MobileNumber
    INTO p_userid, p_fullname, p_mobile
    FROM Users
    WHERE Username = p_username 
    AND Password = p_password 
    AND IsActive = 1;
    
    -- Update last login date
    UPDATE Users 
    SET LastLoginDate = SYSDATE 
    WHERE UserId = p_userid;
    
    COMMIT;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RAISE_APPLICATION_ERROR(-20003, 'Invalid username or password');
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20004, 'Error validating login: ' || SQLERRM);
END ValidateLogin;
/

-- 2. OTP Management Procedures

-- Procedure to generate and store OTP
CREATE OR REPLACE PROCEDURE GenerateOTP(
    p_mobile IN VARCHAR2,
    p_purpose IN VARCHAR2,
    p_otp OUT VARCHAR2
)
AS
    v_userid NUMBER;
    v_otp_code VARCHAR2(6);
BEGIN
    -- Get user ID by mobile number
    SELECT UserId INTO v_userid
    FROM Users
    WHERE MobileNumber = p_mobile;
    
    -- Generate 6-digit OTP
    v_otp_code := LPAD(DBMS_RANDOM.VALUE(0, 999999), 6, '0');
    
    -- Store OTP
    INSERT INTO UserOTP (UserId, OTPCode, ExpiryTime, Purpose)
    VALUES (v_userid, v_otp_code, SYSDATE + 5/1440, p_purpose); -- 5 minutes expiry
    
    p_otp := v_otp_code;
    COMMIT;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RAISE_APPLICATION_ERROR(-20005, 'Mobile number not found');
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20006, 'Error generating OTP: ' || SQLERRM);
END GenerateOTP;
/

-- Procedure to validate OTP
CREATE OR REPLACE PROCEDURE ValidateOTP(
    p_mobile IN VARCHAR2,
    p_otp IN VARCHAR2,
    p_purpose IN VARCHAR2,
    p_isvalid OUT NUMBER
)
AS
    v_count NUMBER;
BEGIN
    SELECT COUNT(*)
    INTO v_count
    FROM UserOTP uo
    JOIN Users u ON uo.UserId = u.UserId
    WHERE u.MobileNumber = p_mobile
    AND uo.OTPCode = p_otp
    AND uo.Purpose = p_purpose
    AND uo.IsUsed = 0
    AND uo.ExpiryTime > SYSDATE;
    
    IF v_count > 0 THEN
        -- Mark OTP as used
        UPDATE UserOTP uo
        SET IsUsed = 1
        WHERE UserId = (SELECT UserId FROM Users WHERE MobileNumber = p_mobile)
        AND OTPCode = p_otp
        AND Purpose = p_purpose;
        
        p_isvalid := 1;
        COMMIT;
    ELSE
        p_isvalid := 0;
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20007, 'Error validating OTP: ' || SQLERRM);
END ValidateOTP;
/

-- 3. Workout Management Procedures

-- Procedure to log a workout
CREATE OR REPLACE PROCEDURE LogWorkout(
    p_userid IN NUMBER,
    p_workoutid IN NUMBER,
    p_duration IN NUMBER,
    p_calories IN NUMBER,
    p_notes IN VARCHAR2,
    p_logid OUT NUMBER
)
AS
BEGIN
    INSERT INTO WorkoutLogs (UserId, WorkoutId, Duration, CaloriesBurned, Notes)
    VALUES (p_userid, p_workoutid, p_duration, p_calories, p_notes)
    RETURNING LogId INTO p_logid;
    
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20008, 'Error logging workout: ' || SQLERRM);
END LogWorkout;
/

-- Procedure to get user workout statistics
CREATE OR REPLACE PROCEDURE GetUserWorkoutStats(
    p_userid IN NUMBER,
    p_days IN NUMBER DEFAULT 30,
    p_total_workouts OUT NUMBER,
    p_total_calories OUT NUMBER,
    p_avg_duration OUT NUMBER
)
AS
BEGIN
    SELECT 
        COUNT(*),
        NVL(SUM(CaloriesBurned), 0),
        NVL(AVG(Duration), 0)
    INTO p_total_workouts, p_total_calories, p_avg_duration
    FROM WorkoutLogs
    WHERE UserId = p_userid
    AND LogDate >= SYSDATE - p_days
    AND Status = 'Completed';
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20009, 'Error getting workout stats: ' || SQLERRM);
END GetUserWorkoutStats;
/

-- 4. Diet Management Procedures

-- Procedure to log a meal
CREATE OR REPLACE PROCEDURE LogMeal(
    p_userid IN NUMBER,
    p_mealid IN NUMBER,
    p_mealtype IN VARCHAR2,
    p_notes IN VARCHAR2,
    p_logid OUT NUMBER
)
AS
    v_calories NUMBER;
    v_protein NUMBER;
    v_carbs NUMBER;
    v_fat NUMBER;
    v_fiber NUMBER;
BEGIN
    -- Get meal nutritional info
    SELECT Calories, Protein, Carbohydrates, Fat, Fiber
    INTO v_calories, v_protein, v_carbs, v_fat, v_fiber
    FROM Meals
    WHERE MealId = p_mealid;
    
    INSERT INTO MealLogs (UserId, MealId, MealType, Calories, Protein, Carbohydrates, Fat, Fiber, Notes)
    VALUES (p_userid, p_mealid, p_mealtype, v_calories, v_protein, v_carbs, v_fat, v_fiber, p_notes)
    RETURNING LogId INTO p_logid;
    
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20010, 'Error logging meal: ' || SQLERRM);
END LogMeal;
/

-- Procedure to get user nutrition statistics
CREATE OR REPLACE PROCEDURE GetUserNutritionStats(
    p_userid IN NUMBER,
    p_days IN NUMBER DEFAULT 30,
    p_total_calories OUT NUMBER,
    p_avg_protein OUT NUMBER,
    p_avg_carbs OUT NUMBER,
    p_avg_fat OUT NUMBER
)
AS
BEGIN
    SELECT 
        NVL(SUM(Calories), 0),
        NVL(AVG(Protein), 0),
        NVL(AVG(Carbohydrates), 0),
        NVL(AVG(Fat), 0)
    INTO p_total_calories, p_avg_protein, p_avg_carbs, p_avg_fat
    FROM MealLogs
    WHERE UserId = p_userid
    AND LogDate >= SYSDATE - p_days;
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20011, 'Error getting nutrition stats: ' || SQLERRM);
END GetUserNutritionStats;
/

-- 5. Rewards Management Procedures

-- Procedure to award points to user
CREATE OR REPLACE PROCEDURE AwardPoints(
    p_userid IN NUMBER,
    p_rewardid IN NUMBER,
    p_points IN NUMBER,
    p_notes IN VARCHAR2
)
AS
BEGIN
    INSERT INTO UserRewards (UserId, RewardId, PointsEarned, Notes)
    VALUES (p_userid, p_rewardid, p_points, p_notes);
    
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20012, 'Error awarding points: ' || SQLERRM);
END AwardPoints;
/

-- Procedure to log daily streak
CREATE OR REPLACE PROCEDURE LogDailyStreak(
    p_userid IN NUMBER,
    p_workout_completed IN NUMBER,
    p_diet_followed IN NUMBER,
    p_points IN NUMBER,
    p_notes IN VARCHAR2
)
AS
BEGIN
    INSERT INTO DailyStreaks (UserId, WorkoutCompleted, DietFollowed, PointsEarned, Notes)
    VALUES (p_userid, p_workout_completed, p_diet_followed, p_points, p_notes);
    
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20013, 'Error logging daily streak: ' || SQLERRM);
END LogDailyStreak;
/

-- Procedure to get user reward statistics
CREATE OR REPLACE PROCEDURE GetUserRewardStats(
    p_userid IN NUMBER,
    p_total_points OUT NUMBER,
    p_current_streak OUT NUMBER,
    p_longest_streak OUT NUMBER
)
AS
BEGIN
    -- Get total points
    SELECT NVL(SUM(PointsEarned), 0)
    INTO p_total_points
    FROM UserRewards
    WHERE UserId = p_userid;
    
    -- Get current streak (simplified calculation)
    SELECT NVL(COUNT(*), 0)
    INTO p_current_streak
    FROM DailyStreaks
    WHERE UserId = p_userid
    AND StreakDate >= SYSDATE - 7
    AND (WorkoutCompleted = 1 OR DietFollowed = 1);
    
    -- Get longest streak (simplified calculation)
    SELECT NVL(MAX(streak_count), 0)
    INTO p_longest_streak
    FROM (
        SELECT COUNT(*) as streak_count
        FROM DailyStreaks
        WHERE UserId = p_userid
        AND (WorkoutCompleted = 1 OR DietFollowed = 1)
        GROUP BY TRUNC(StreakDate)
    );
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20014, 'Error getting reward stats: ' || SQLERRM);
END GetUserRewardStats;
/

-- 6. Data Retrieval Procedures

-- Procedure to get user dashboard data
CREATE OR REPLACE PROCEDURE GetUserDashboardData(
    p_userid IN NUMBER,
    p_recent_workouts OUT SYS_REFCURSOR,
    p_recent_meals OUT SYS_REFCURSOR,
    p_rewards OUT SYS_REFCURSOR
)
AS
BEGIN
    -- Get recent workouts
    OPEN p_recent_workouts FOR
        SELECT wl.LogId, w.Name as WorkoutName, wl.Duration, wl.CaloriesBurned, wl.LogDate
        FROM WorkoutLogs wl
        JOIN Workouts w ON wl.WorkoutId = w.WorkoutId
        WHERE wl.UserId = p_userid
        ORDER BY wl.LogDate DESC
        FETCH FIRST 5 ROWS ONLY;
    
    -- Get recent meals
    OPEN p_recent_meals FOR
        SELECT ml.LogId, m.Name as MealName, ml.MealType, ml.Calories, ml.LogDate
        FROM MealLogs ml
        JOIN Meals m ON ml.MealId = m.MealId
        WHERE ml.UserId = p_userid
        ORDER BY ml.LogDate DESC
        FETCH FIRST 5 ROWS ONLY;
    
    -- Get recent rewards
    OPEN p_rewards FOR
        SELECT ur.UserRewardId, r.Name as RewardName, ur.PointsEarned, ur.EarnedDate
        FROM UserRewards ur
        JOIN Rewards r ON ur.RewardId = r.RewardId
        WHERE ur.UserId = p_userid
        ORDER BY ur.EarnedDate DESC
        FETCH FIRST 5 ROWS ONLY;
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20015, 'Error getting dashboard data: ' || SQLERRM);
END GetUserDashboardData;
/

-- 7. Utility Procedures

-- Procedure to check if user exists
CREATE OR REPLACE FUNCTION UserExists(p_username IN VARCHAR2) RETURN NUMBER
AS
    v_count NUMBER;
BEGIN
    SELECT COUNT(*)
    INTO v_count
    FROM Users
    WHERE Username = p_username;
    
    RETURN v_count;
EXCEPTION
    WHEN OTHERS THEN
        RETURN 0;
END UserExists;
/

-- Procedure to get user by ID
CREATE OR REPLACE PROCEDURE GetUserById(
    p_userid IN NUMBER,
    p_username OUT VARCHAR2,
    p_email OUT VARCHAR2,
    p_fullname OUT VARCHAR2,
    p_mobile OUT VARCHAR2
)
AS
BEGIN
    SELECT Username, Email, FullName, MobileNumber
    INTO p_username, p_email, p_fullname, p_mobile
    FROM Users
    WHERE UserId = p_userid;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RAISE_APPLICATION_ERROR(-20016, 'User not found');
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20017, 'Error getting user: ' || SQLERRM);
END GetUserById;
/

-- Clean up expired OTPs (can be called by a scheduled job)
CREATE OR REPLACE PROCEDURE CleanupExpiredOTPs
AS
BEGIN
    DELETE FROM UserOTP
    WHERE ExpiryTime < SYSDATE;
    
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20018, 'Error cleaning up OTPs: ' || SQLERRM);
END CleanupExpiredOTPs;
/

COMMIT; 