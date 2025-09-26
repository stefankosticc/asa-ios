import { User } from "./auth";
import authAxios from "./authAxios";

export interface UpdateUserBiographyRequest {
  biography: string;
}

export interface UserSearchResponse {
  id: number;
  name: string;
  userName: string;
  profilePhoto: string;
}

export interface UpdateUserProfileRequest {
  name: string;
  removePhoto: boolean;
}

export const updateUserBiography = async (
  request: UpdateUserBiographyRequest
): Promise<void> => {
  try {
    await authAxios.put(`/user/biography`, request);
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred while updating users biography.";
    console.error("Error:", message);
  }
};

export async function searchArtists(
  searchString: string
): Promise<UserSearchResponse[]> {
  const response = await authAxios.get(`/users/search`, {
    params: { searchString },
  });
  return response.data;
}

export async function getUserByUsername(username: string): Promise<User> {
  const response = await authAxios.get(`/user/username/${username}`);
  return response.data;
}

export async function updateUserProfile(
  request: UpdateUserProfileRequest,
  profilePhoto: File | null
): Promise<void> {
  try {
    const formData = new FormData();
    for (const key in request) {
      const value = (request as any)[key];
      formData.append(key, value !== null ? value.toString() : "");
    }

    if (profilePhoto) {
      formData.append("profilePhoto", profilePhoto);
    }

    await authAxios.put(`/user`, formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred while updating user profile.";
    console.error("Error:", message);
  }
}
